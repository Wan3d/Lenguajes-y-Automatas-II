; Archivo: prueba.cpp
; Fecha y hora: 02/03/2025 13:02:30
; ----------------------------------
segment .text
global main
main:
; Asignación de x26
	MOV EAX, [200]
	PUSH EAX
	POP EAX
	MOV [x26], EAX
	MOV AL, [x26]
	INC AL
	MOV [x26], AL
	DEC x26
	RET
section .data
x26 DW 200
