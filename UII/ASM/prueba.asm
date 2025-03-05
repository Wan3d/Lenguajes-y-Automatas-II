; Archivo: prueba.cpp
; Fecha y hora: 04/03/2025 19:22:14
; ----------------------------------
segment .text
global main
main:
; Asignación de x
	MOV EAX, 10
	PUSH EAX
	POP EAX
	MOV [x], EAX
; Do
jump_do_1:
	MOV AL, [x]
	INC AL
	MOV [x], AL
	PRINT_STRING "11"
	NEWLINE
; Expresión 1
	MOV EAX, x
	PUSH EAX
; Expresión 2
	MOV EAX, 13
	PUSH EAX
	POP EBX
	POP EAX
	CMP EAX, EBX
	JB jump_do_1
	RET
section .data
x DD 0
