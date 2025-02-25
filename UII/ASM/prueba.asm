; Archivo: prueba.cpp
; Fecha y hora: 25/02/2025 12:37:18
;----------------------------------
SEGMENT .TEXT
GLOBAL MAIN
MAIN:
; Asignación de x26
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
    PUSH EAX
    POP EBX
    POP EAX
    SUB EAX, EBX
    PUSH EAX
    POP EAX
    MOV DWORD[x26], EAX
SECTION .DATA
x26 DB 0
