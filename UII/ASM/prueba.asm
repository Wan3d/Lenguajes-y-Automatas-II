; Archivo: prueba.cpp
; Fecha y hora: 10/03/2025 0:16:44
; ----------------------------------
%include "io.inc"
segment .text
global main
main:
; Asignación de j
	MOV EAX, 9
	PUSH EAX
	POP EAX
	MOV DWORD[j], EAX
; Asignación de i
	MOV EAX, 5
	PUSH EAX
	POP EAX
	MOV DWORD[i], EAX
	MOV EAX, 5
	PUSH EAX
	POP EAX
	MOV DWORD[i], EAX
; Inicio de For 1
jump_For_1:
; Expresión 1
	MOV EAX, DWORD[i]
	PUSH EAX
; Expresión 2
	MOV EAX, DWORD[j]
	PUSH EAX
	POP EBX
	POP EAX
	CMP EAX, EBX
	JA end_For_1
	MOV EAX, DWORD[i]
	INC EAX
	MOV DWORD[i], EAX
; Inicio If/Else 1
jump_if_1:
; Expresión 1
	MOV EAX, DWORD[i]
	PUSH EAX
	MOV EAX, 2
	PUSH EAX
	POP EBX
	POP EAX
	XOR EDX, EDX
	DIV EBX
	PUSH EDX
; Expresión 2
	MOV EAX, 0
	PUSH EAX
	POP EBX
	POP EAX
	CMP EAX, EBX
	JNE jump_else_1
	PRINT_DEC 4, i
	NEWLINE
	JMP fin_Condicion_1
jump_else_1:
fin_Condicion_1:
; Fin If/Else 1
	MOV EAX, DWORD[i]
	INC EAX
	MOV DWORD[i], EAX
	JMP jump_For_1
end_For_1:
; Fin de For 1
	RET
section .data
j DD 9
i DD 7
