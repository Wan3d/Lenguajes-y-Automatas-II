; Archivo: prueba.cpp
; Fecha y hora: 05/03/2025 22:29:10
; ----------------------------------
%include "io.inc"
segment .text
global main
main:
; Asignación de x
	MOV EAX, 5
	PUSH EAX
	POP EAX
	MOV DWORD[x], EAX
	MOV EAX, 1
	PUSH EAX
	POP EAX
	MOV DWORD[i], EAX
; Inicio de For 1
jump_For_1:
; Expresión 1
	MOV EAX, DWORD[x]
	PUSH EAX
; Expresión 2
	MOV EAX, 10
	PUSH EAX
	POP EBX
	POP EAX
	CMP EAX, EBX
	JAE end_For_1
	MOV EAX, DWORD[i]
	INC EAX
	MOV DWORD[i], EAX
	MOV EAX, DWORD[x]
	INC EAX
	MOV DWORD[x], EAX
	JMP jump_For_1:
end_For_1:
; Fin de For 1
	MOV EAX, 1
	PUSH EAX
	POP EAX
	MOV DWORD[i], EAX
; Inicio de For 2
jump_For_2:
; Expresión 1
	MOV EAX, DWORD[x]
	PUSH EAX
; Expresión 2
	MOV EAX, 10
	PUSH EAX
	POP EBX
	POP EAX
	CMP EAX, EBX
	JAE end_For_2
	MOV EAX, DWORD[i]
	INC EAX
	MOV DWORD[i], EAX
	MOV EAX, DWORD[x]
	INC EAX
	MOV DWORD[x], EAX
	JMP jump_For_2:
end_For_2:
; Fin de For 2
	RET
section .data
x DD 0
i DD 0
