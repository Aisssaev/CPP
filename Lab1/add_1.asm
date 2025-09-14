        IDEAL
        DOSSEG
        MODEL   small
        STACK   256

        DATASEG
num1        DW      25      
num2        DW      7       
sum         DW
diff        DW
prod        DW
quot        DW
rem         DW
exitCode    DB      0       

        CODESEG
Start:
        mov     ax, @data  
        mov     ds, ax

; ------- ДОДАВННЯ -------
        mov     ax, [num1]
        add     ax, [num2]  ; AX = num1 + num2
        mov     [sum], ax  

; ------- ВІДНІМАННЯ -------
        mov     ax, [num1]
        sub     ax, [num2]  ; AX = num1 - num2
        mov     [diff], ax

; ------- МНОЖЕННЯ -------
        mov     ax, [num1]
        mov     bx, [num2]
        mul     bx          ; AX = AX * BX (16×16 = 32)
        mov     [prod], ax

; ------- ДІЛЕННЯ -------
        mov     ax, [num1]
        cwd                
        mov     bx, [num2]
        div     bx         
        mov     [quot], ax
        mov     [rem], dx

;
Exit:
        mov     ah, 4Ch     ; DOS: Terminate with return code
        mov     al, [exitCode]
        int     21h

        END     Start
