# GorillaScript Documentation

GorillaScript is a custom scripting language designed for Gorilla Tag custom maps, inspired by VRChat's Udon. It allows map creators to add custom behavior to their maps using a straightforward scripting system.

---

## Table of Contents
- [Introduction](#introduction)
- [Getting Started](#getting-started)
- [Language Syntax](#language-syntax)
- [Built-in Properties](#built-in-properties)
- [Built-in Functions](#built-in-functions)
- [Control Flow](#control-flow)
- [Variable Types](#variable-types)
- [Defining Functions](#defining-functions)
- [Examples](#examples)
- [Error Handling](#error-handling)

---

## Introduction

GorillaScript provides creators with a simple scripting framework for defining behaviors in Gorilla Tag custom maps. Scripts can define `Start` and `Update` functions, utilize game-specific properties, and perform basic operations like logging, conditional statements, and variable manipulation.

GorillaScript has no comment support, NO COMMENTS IN CODE!!!!!

---

## Getting Started

1. Add the `GorillaScriptManager` component to your custom map in Unity.
2. Write your scripts in the format provided below.
3. Register your scripts with `RegisterScript` and call them using `ExecuteScript` or predefined lifecycle functions (`Start` and `Update`).

---

## Language Syntax

GorillaScript uses a simple, Python-like syntax.

```gorillascript
def Start:
    Log("Script started!");
    bool isMeTagged = IsMeTagged;
    if (isMeTagged) {
        Log("You are tagged!");
    }

def Update:
    int taggedPlayers = TaggedPlayers;
    Log("Number of tagged players: " + taggedPlayers);
```

---

## Built-in Properties

GorillaScript provides the following built-in properties:

- **IsMeTagged** (`bool`)  
  Checks if the local player is tagged.
  
- **TaggedPlayers** (`int`)  
  Returns the number of currently tagged players.

- **PlayerPos** (`vec3`)  
  The local player’s position in world space.

- **PlayerRot** (`quat`)  
  The local player’s rotation in world space.

---

## Built-in Functions

### Log(message)
Logs a message to the Unity console.  
- **Parameters:**  
  `message` (string): The message to log.

Example:
```gorillascript
Log("This is a test message!");
```

---

## Control Flow

### `if` Statements
Conditionally execute code based on a boolean expression.

Syntax:
```gorillascript
if (condition) {
    // Code to execute if condition is true
}
```

Example:
```gorillascript
if (IsMeTagged) {
    Log("You are tagged!");
}
```

---

## Variable Types

GorillaScript supports the following variable types:

- **bool**: Boolean (`true` or `false`)
- **int**: Integer
- **vec3**: 3D vector (`Vector3` in Unity)
- **quat**: Quaternion (`Quaternion` in Unity)

Example:
```gorillascript
bool isMeTagged = IsMeTagged;
int taggedPlayers = TaggedPlayers;
vec3 playerPosition = PlayerPos;
quat playerRotation = PlayerRot;
```

---

## Defining Functions

GorillaScript allows defining two functions:

1. **Start**  
   Executed when the script is initialized.

2. **Update**  
   Executed every frame.

Syntax:
```gorillascript
def Start:
    // Code to execute at the start

def Update:
    // Code to execute every frame
```

---

## Examples

### Example 1: Logging Player Status
```gorillascript
def Start:
    Log("Starting script...");
    if (IsMeTagged) {
        Log("You are tagged!");
    } else {
        Log("You are not tagged!");
    }

def Update:
    Log("Player position: " + PlayerPos);
```

```gorillascript
### Example 2: Counting Tagged Players
def Start:
    Log("Initial tagged player count: " + TaggedPlayers);

def Update:
    int currentTagged = TaggedPlayers;
    Log("Currently tagged players: " + currentTagged);
```

---

## Error Handling

Errors in scripts will be logged to the Unity console with details about the error.  
Use `Log` statements for debugging and verifying script behavior.

**Common Errors:**
- Undefined variables
- Syntax errors in expressions
- Unsupported operations

Example Error Log:
Error executing script 'Example': Variable 'playerPos' not defined.
