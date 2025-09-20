.386 

.model flat, stdcall 

option casemap:none 

 

EXTERN MessageBoxA@16:PROC 

EXTERN ExitProcess@4:PROC 

 

includelib user32.lib 

includelib kernel32.lib 

 

.data 

    caption db "ASM Program",0 

    message db "Привіт із віконного MASM!",0 

 

.code 

main: 

    push 0 

    push offset caption 

    push offset message 

    push 0 

    call MessageBoxA@16 

 

    push 0 

    call ExitProcess@4 

end main