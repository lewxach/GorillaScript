using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaScript
{
    public class GorillaScriptManager : MonoBehaviour
    {
        // manager things
        private Dictionary<string, object> variables = new Dictionary<string, object>();
        private Dictionary<string, string[]> scripts = new Dictionary<string, string[]>();
        private Dictionary<string, List<string>> functions = new Dictionary<string, List<string>>();

        // gorilla script propsz
        private bool IsMeTagged => FindObjectOfType<GorillaTagManager>().MyMatIndex(NetworkSystem.Instance.LocalPlayer).IsHigherThanOrEqualTo(1);
        private int TaggedPlayers => FindObjectOfType<GorillaTagManager>().currentInfected.CountOfValid();
        private Vector3 PlayerPos => GorillaLocomotion.Player.Instance.transform.position;
        private Quaternion PlayerRot => GorillaLocomotion.Player.Instance.transform.rotation;

        void Start()
        {
            string[] exampleScript = new string[]
            {
                "def Start:",
                "    Log(\"Script Start!\");",
                "    bool isMeTagged = IsMeTagged;",
                "    Log(\"You are tagged: \" + isMeTagged);",
                "def Update:",
                "    int taggedPlayers = TaggedPlayers;",
                "    Log(\"Tagged players count: \" + taggedPlayers);"
            };

            RegisterScript("Example", exampleScript);
            ExecuteFunction("Start");
        }

        void Update()
        {
            ExecuteFunction("Update");
        }

        public void RegisterScript(string scriptName, string[] script)
        {
            scripts[scriptName] = script;

            string currentFunction = null;

            foreach (string line in script)
            {
                string trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("def "))
                {
                    string funcName = trimmedLine.Substring(4).Split(':')[0].Trim();
                    if (funcName != "Start" && funcName != "Update")
                        throw new Exception($"Unsupported function '{funcName}'. Only 'Start' and 'Update' are allowed.");

                    currentFunction = funcName;
                    if (!functions.ContainsKey(currentFunction))
                        functions[currentFunction] = new List<string>();
                }
                else if (currentFunction != null)
                {
                    functions[currentFunction].Add(trimmedLine);
                }
            }
        }

        public void ExecuteFunction(string functionName)
        {
            if (!functions.ContainsKey(functionName))
            {
                Debug.LogError($"Function '{functionName}' not found.");
                return;
            }

            List<string> functionLines = functions[functionName];
            foreach (string line in functionLines)
            {
                try
                {
                    ExecuteLine(line);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error in function '{functionName}': {ex.Message}");
                }
            }
        }

        private void ExecuteLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            if (line.StartsWith("Log("))
            {
                string message = line.Substring(4, line.Length - 5).Trim();
                Debug.Log(EvaluateExpression(message));
                return;
            }

            if (line.StartsWith("if ("))
            {
                int conditionStart = line.IndexOf('(') + 1;
                int conditionEnd = line.IndexOf(')');
                string condition = line.Substring(conditionStart, conditionEnd - conditionStart).Trim();
                bool conditionResult = (bool)EvaluateExpression(condition);

                if (conditionResult && line.Contains("{"))
                {
                    int startIndex = line.IndexOf('{') + 1;
                    int endIndex = line.LastIndexOf('}');
                    string innerCode = line.Substring(startIndex, endIndex - startIndex).Trim();
                    ExecuteLine(innerCode);
                }
                return;
            }

            if (line.Contains("="))
            {
                string[] parts = line.Split(new[] { '=' }, 2);
                string variableDeclaration = parts[0].Trim();
                string expression = parts[1].Trim();

                object value = EvaluateExpression(expression);

                if (variableDeclaration.Contains(" "))
                {
                    string[] varParts = variableDeclaration.Split(' ');
                    string type = varParts[0];
                    string varName = varParts[1];
                    variables[varName] = ConvertType(type, value);
                }
                else
                {
                    string varName = variableDeclaration;
                    if (!variables.ContainsKey(varName))
                        throw new Exception($"Variable '{varName}' not defined.");

                    variables[varName] = value;
                }

                return;
            }

            throw new Exception($"Unknown line format: {line}");
        }

        private object EvaluateExpression(string expression)
        {
            if (expression.StartsWith("\"") && expression.EndsWith("\""))
                return expression.Substring(1, expression.Length - 2);

            if (bool.TryParse(expression, out bool boolResult))
                return boolResult;

            if (int.TryParse(expression, out int intResult))
                return intResult;

            if (variables.ContainsKey(expression))
                return variables[expression];

            if (expression.Equals("IsMeTagged")) return IsMeTagged;
            if (expression.Equals("TaggedPlayers")) return TaggedPlayers;
            if (expression.Equals("PlayerPos")) return PlayerPos;
            if (expression.Equals("PlayerRot")) return PlayerRot;

            throw new Exception($"Unable to evaluate expression: {expression}");
        }

        private object ConvertType(string type, object value)
        {
            switch (type)
            {
                case "bool": return Convert.ToBoolean(value);
                case "int": return Convert.ToInt32(value);
                case "vec3": return (Vector3)value;
                case "quat": return (Quaternion)value;
                default: throw new Exception($"Unknown type: {type}");
            }
        }
    }
}