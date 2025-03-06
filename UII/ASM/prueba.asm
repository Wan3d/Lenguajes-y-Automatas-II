; Archivo: prueba.cpp
; Fecha y hora: 05/03/2025 23:23:21
; ----------------------------------
%include "io.inc"
segment .text
global main
main:
; Asignación de x
	MOV EAX, 60
	PUSH EAX
	POP EAX
	MOV DWORD[x], EAX
; Inicio If/Else 1
jump_if_1:
; Expresión 1
	MOV EAX, DWORD[x]
	PUSH EAX
; Expresión 2
	MOV EAX, 60
	PUSH EAX
	POP EBX
	POP EAX
	CMP EAX, EBX
	JNE jump_else_1
	MOV EAX, DWORD[x]
	INC EAX
	MOV DWORD[x], EAX
	JMP fin_if_1
jump_else_1:
	MOV EAX, DWORD[x]
	DEC EAX
	MOV DWORD[x], EAX
fin_if_1:
; Fin If/Else 1
	RET
section .data
x DD 0
