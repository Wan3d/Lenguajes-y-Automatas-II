; Archivo: prueba.cpp
; Fecha y hora: 17/03/2025 22:50:07
; ----------------------------------
%include "io.inc"
segment .text
global main
main:
; Asignación de x
	MOV EAX, 0
	PUSH EAX
	POP EAX
	MOV DWORD[x], EAX
; Asignación de y
	MOV EAX, 10
	PUSH EAX
	POP EAX
	MOV DWORD[y], EAX
; Asignación de z
	MOV EAX, 2
	PUSH EAX
	POP EAX
	MOV DWORD[z], EAX
	MOV EAX, 400
	PUSH EAX
	MOV EAX, 200
	PUSH EAX
	POP EBX
	POP EAX
	ADD EAX, EBX
	PUSH EAX
	POP EAX
	MOV EBX, 256
	XOR EDX, EDX
	DIV EBX
	MOV EAX, EDX
	PUSH EAX
	POP EAX
	MOV DWORD[c], EAX
	RET
section .data
altura DD 0
x DD 0
y DD 0
z DD 0
c DD 0
