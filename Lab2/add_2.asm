extrn ExitProcess: PROC
extrn GetStdHandle: PROC
extrn WriteConsoleA: PROC
extrn ReadConsoleA: PROC

.data
    msgWelcome   db "Welcome to the x64 assembly program!", 13, 10, 0
    msgPrompt    db "Please enter your name: ", 0
    msgHello     db "Hello, ", 0
    msgResult    db "Calculation result (5 + 3 * 2): ", 0
    msgNewLine   db 13, 10, 0

    inputBuffer  db 100 dup(0)
    bytesRead    dq 0
    outputBuffer db 20 dup(0)

    hStdInput    dq 0
    hStdOutput   dq 0

    resultValue  dq 0       ; Variable to store the result

.code
main PROC
    sub rsp, 28h            ; Reserve shadow space + align the stack

    ; Get standard handles
    mov rcx, -11            ; STD_OUTPUT_HANDLE
    call GetStdHandle
    mov qword ptr [hStdOutput], rax

    mov rcx, -10            ; STD_INPUT_HANDLE
    call GetStdHandle
    mov qword ptr [hStdInput], rax

    ; Print welcome message
    lea rcx, msgWelcome
    call PrintString

    ; Print input prompt
    lea rcx, msgPrompt
    call PrintString

    ; Read user input
    mov rcx, qword ptr [hStdInput]
    lea rdx, inputBuffer
    mov r8d, 100
    lea r9, bytesRead
    mov qword ptr [rsp+20h], 0
    call ReadConsoleA

    ; Remove CR/LF characters
    mov rax, qword ptr [bytesRead]
    cmp rax, 2
    jb short skipTrim
    sub rax, 2
    lea rdx, inputBuffer
    mov byte ptr [rdx+rax], 0
    jmp short doneTrim
skipTrim:
    mov byte ptr [inputBuffer], 0
doneTrim:

    ; Print "Hello, <name>"
    lea rcx, msgHello
    call PrintString
    lea rcx, inputBuffer
    call PrintString

    ; Perform calculation: 5 + 3 * 2
    mov rax, 5
    mov rbx, 3
    mov rcx, 2
    imul rbx, rcx           ; rbx = 3 * 2 = 6
    add rax, rbx            ; rax = 5 + 6 = 11
    mov [resultValue], rax  ; Store result

    ; Print result
    lea rcx, msgNewLine
    call PrintString
    lea rcx, msgResult
    call PrintString

    mov rcx, [resultValue]  ; Load result
    call PrintNumber

    lea rcx, msgNewLine
    call PrintString

    ; Exit the program
    xor rcx, rcx
    call ExitProcess
main ENDP

;-------------------------------------------------------------
; PrintString: prints a null-terminated string to the console
; RCX = address of the string
;-------------------------------------------------------------
PrintString PROC
    push rbp
    mov rbp, rsp
    sub rsp, 20h

    mov rsi, rcx             ; Pointer to the string

    ; Calculate string length
    xor rcx, rcx
    mov rdi, rsi
countLoop:
    cmp byte ptr [rdi], 0
    je countDone
    inc rdi
    inc rcx
    jmp countLoop
countDone:

    ; Call WriteConsoleA to print the string
    mov rdx, rsi             ; String pointer
    mov r8d, ecx             ; String length
    lea r9, bytesRead
    mov rcx, qword ptr [hStdOutput]
    mov qword ptr [rsp+20h], 0
    call WriteConsoleA

    add rsp, 20h
    pop rbp
    ret
PrintString ENDP

;-------------------------------------------------------------
; PrintNumber: prints an integer in decimal format
; RCX = number to print
;-------------------------------------------------------------
PrintNumber PROC
    push rbp
    mov rbp, rsp
    sub rsp, 30h

    mov rax, rcx
    lea rdi, outputBuffer + 19
    mov byte ptr [rdi], 0
    mov rbx, 10
convertLoop:
    dec rdi
    xor rdx, rdx
    div rbx
    add dl, '0'
    mov [rdi], dl
    test rax, rax
    jnz convertLoop

    mov rcx, rdi
    call PrintString

    add rsp, 30h
    pop rbp
    ret
PrintNumber ENDP

END
