MODEL SMALL
STACK 256
DATASEG
CODESEG
start:
    mov ax, 0013h
    int 10h
    mov cx, 100
    mov dx, 100
draw:
    dec dx
    dec cx
    mov al, 26
    mov ah, 0Ch
    int 10h
loop draw
ex:
    mov ah, 1
    int 16h
jz ex
    mov ax, 0003h
    int 10h
    mov ah, 04Ch
    int 021h
END Start