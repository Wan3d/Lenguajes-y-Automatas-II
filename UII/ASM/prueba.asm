; Archivo: prueba.cpp
; Fecha y hora: 11/03/2025 11:55:32
; ----------------------------------
%include "io.inc"
segment .text
global main
main:
; Asignación de a
	MOV EAX, 260
	PUSH EAX
	POP EAX
	MOV DWORD[a], EAX
; Asignación de b
	MOV EAX, DWORD[a]
	PUSH EAX
	POP EAX
	PUSH EAX
	POP EAX
	MOV DWORD[b], EAX
	RET
section .data
a DD 0
b DD 0
