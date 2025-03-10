; Archivo: prueba.cpp
; Fecha y hora: 10/03/2025 10:01:36
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
	MOV EAX, 100
	PUSH EAX
	MOV EAX, 200
	PUSH EAX
	POP EBX
	POP EAX
	ADD EAX, EBX
	PUSH EAX
	POP EAX
	MOV DWORD[c], EAX
	PRINT_STRING str1
	NEWLINE
	GET_DEC 4, altura
	NEWLINE
	MOV EAX, 3
	PUSH EAX
	MOV EAX, DWORD[altura]
	PUSH EAX
	POP EBX
	POP EAX
	ADD EAX, EBX
	PUSH EAX
	MOV EAX, 8
	PUSH EAX
	POP EBX
	POP EAX
	MUL EBX
	PUSH EAX
	MOV EAX, 10
	PUSH EAX
	MOV EAX, 4
	PUSH EAX
	POP EBX
	POP EAX
	SUB EAX, EBX
	PUSH EAX
	MOV EAX, 2
	PUSH EAX
	POP EBX
	POP EAX
	DIV EBX
	PUSH EAX
	POP EBX
	POP EAX
	SUB EAX, EBX
	PUSH EAX
	POP EAX
	MOV DWORD[x], EAX
	MOV EAX, DWORD[x]
	DEC EAX
	MOV DWORD[x], EAX
	MOV EAX, DWORD[altura]
	PUSH EAX
	MOV EAX, 8
	PUSH EAX
	POP EBX
	POP EAX
	MUL EBX
	PUSH EAX
	MOV EAX, DWORD[x]
	ADD EAX, 40
	MOV DWORD[x], EAX
	MOV EAX, 2
	PUSH EAX
	MOV EAX, DWORD[x]
	MOV EBX, 2
	MUL EBX
	MOV DWORD[x], EAX
	MOV EAX, DWORD[y]
	PUSH EAX
	MOV EAX, 6
	PUSH EAX
	POP EBX
	POP EAX
	SUB EAX, EBX
	PUSH EAX
	MOV EAX, DWORD[x]
	MOV ECX, 4
	XOR EDX, EDX
	DIV ECX
	MOV DWORD[x], EAX
	MOV EAX, 1
	PUSH EAX
	POP EAX
	MOV DWORD[i], EAX
; Inicio de For 2
jump_For_1:
; Expresión 1
	MOV EAX, DWORD[i]
	PUSH EAX
; Expresión 2
	MOV EAX, DWORD[altura]
	PUSH EAX
	POP EBX
	POP EAX
	CMP EAX, EBX
	JA end_For_1
	MOV EAX, DWORD[i]
	INC EAX
	MOV DWORD[i], EAX
	MOV EAX, 1
	PUSH EAX
	POP EAX
	MOV DWORD[j], EAX
; Inicio de For 3
jump_For_2:
; Expresión 1
	MOV EAX, DWORD[j]
	PUSH EAX
; Expresión 2
	MOV EAX, DWORD[i]
	PUSH EAX
	POP EBX
	POP EAX
	CMP EAX, EBX
	JA end_For_2
	MOV EAX, DWORD[j]
	INC EAX
	MOV DWORD[j], EAX
; Inicio If/Else 1
jump_if_1:
; Expresión 1
	MOV EAX, DWORD[j]
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
	PRINT_STRING str2
	JMP fin_Condicion_1
jump_else_1:
	PRINT_STRING str3
fin_Condicion_1:
; Fin If/Else 1
	JMP jump_For_2
end_For_2:
; Fin de For 3
	PRINT_STRING str4
	NEWLINE
	JMP jump_For_1
end_For_1:
; Fin de For 3
	MOV EAX, 0
	PUSH EAX
	POP EAX
	MOV DWORD[i], EAX
; Do
jump_do_1:
	PRINT_STRING str5
	MOV EAX, DWORD[i]
	INC EAX
	MOV DWORD[i], EAX
; Expresión 1
	MOV EAX, DWORD[i]
	PUSH EAX
; Expresión 2
	MOV EAX, DWORD[altura]
	PUSH EAX
	MOV EAX, 2
	PUSH EAX
	POP EBX
	POP EAX
	MUL EBX
	PUSH EAX
	POP EBX
	POP EAX
	CMP EAX, EBX
	JB jump_do_1
	PRINT_STRING str6
	NEWLINE
	MOV EAX, 1
	PUSH EAX
	POP EAX
	MOV DWORD[i], EAX
; Inicio de For 4
jump_For_3:
; Expresión 1
	MOV EAX, DWORD[i]
	PUSH EAX
; Expresión 2
	MOV EAX, DWORD[altura]
	PUSH EAX
	POP EBX
	POP EAX
	CMP EAX, EBX
	JA end_For_3
	MOV EAX, DWORD[i]
	INC EAX
	MOV DWORD[i], EAX
	MOV EAX, 1
	PUSH EAX
	POP EAX
	MOV DWORD[j], EAX
jump_While_1:
; Expresión 1
	MOV EAX, DWORD[j]
	PUSH EAX
; Expresión 2
	MOV EAX, DWORD[i]
	PUSH EAX
	POP EBX
	POP EAX
	CMP EAX, EBX
	JA end_While_1
	PRINT_DEC 4, j
	MOV EAX, DWORD[j]
	INC EAX
	MOV DWORD[j], EAX
	JMP jump_While_1
end_While_1:
	PRINT_STRING str7
	NEWLINE
	JMP jump_For_3
end_For_3:
; Fin de For 4
	MOV EAX, 0
	PUSH EAX
	POP EAX
	MOV DWORD[i], EAX
; Do
jump_do_2:
	PRINT_STRING str8
	MOV EAX, DWORD[i]
	INC EAX
	MOV DWORD[i], EAX
; Expresión 1
	MOV EAX, DWORD[i]
	PUSH EAX
; Expresión 2
	MOV EAX, DWORD[altura]
	PUSH EAX
	MOV EAX, 2
	PUSH EAX
	POP EBX
	POP EAX
	MUL EBX
	PUSH EAX
	POP EBX
	POP EAX
	CMP EAX, EBX
	JB jump_do_2
	PRINT_STRING str9
	NEWLINE
	RET
section .data
altura DD 0
i DD 0
j DD 0
x DD 0
y DD 0
z DD 0
c DD 0
str1 DB 'Valor de altura = ', 0
str2 DB '*', 0
str3 DB '-', 0
str4 DB '', 0
str5 DB '-', 0
str6 DB '', 0
str7 DB '', 0
str8 DB '-', 0
str9 DB '', 0
