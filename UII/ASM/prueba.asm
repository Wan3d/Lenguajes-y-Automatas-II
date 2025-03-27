; Archivo: prueba.cpp
; Fecha y hora: 26/03/2025 23:02:04
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
	PRINT_DEC 4, x
	NEWLINE
	RET
section .data
x DW 0
