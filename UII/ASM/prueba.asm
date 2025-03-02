; Archivo: prueba.cpp
; Fecha y hora: 02/03/2025 0:28:35
; ----------------------------------
segment .text
global main
main:
; Asignación de x26
	MOV EAX, [200]
	PUSH EAX
	POP EAX
	MOV [x26], EAX
	INC x26
	RET
section .data
x26 DW 201
