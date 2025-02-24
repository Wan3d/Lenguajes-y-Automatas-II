; Archivo: prueba.cpp
; Fecha y hora: 24/02/2025 11:57:46
----------------------------------
    MOV EAX, 3
    PUSH AX
    MOV EAX, 5
    PUSH AX
    POP EBX
    POP EAX
    ADD EAX, EBX
    PUSH EAX
    MOV EAX, 8
    PUSH AX
    POP EBX
    POP EAX
    MUL EBX
    PUSH AX
    MOV EAX, 10
    PUSH AX
    MOV EAX, 4
    PUSH AX
    POP EBX
    POP EAX
    SUB EAX, EBX
    PUSH EAX
    MOV EAX, 2
    PUSH AX
    POP EBX
    POP EAX
    DIV EBX
    PUSH AL
    POP EBX
    POP EAX
    SUB EAX, EBX
    PUSH EAX
    POP
