.MODEL small
.STACK 256

.DATA
X       DD 1.0, 2.0, 3.0, 4.0   ; масив чисел
N       DW 4                     ; кількість елементів
SUM     DD ?                     ; для зберігання суми

.CODE
START:
    ; Ініціалізація сегмента даних
    mov ax, @data
    mov ds, ax

    ; Ініціалізація FPU
    FINIT

    ; Обнулення суми в ST(0)
    FLDZ                ; ST(0) = 0 (сума)
    xor si, si          ; індекс = 0
    mov cx, [N]         ; кількість елементів

SUM_LOOP:
    FLD DWORD PTR [X+SI] ; Завантажуємо X[i] → ST(0) = X[i], ST(1) = сума
    FADD ST(1), ST(0)     ; ST(1) = ST(1) + ST(0) (сума + X[i])
    FXCH ST(1)             ; Обмінюємо ST(0) і ST(1), сума завжди в ST(0)
    FSTP ST(1)             ; Викидаємо тимчасовий ST(1)
    
    add SI, 4              ; Переходимо до наступного елемента
    loop SUM_LOOP

    ; Збереження суми з ST(0) в пам’ять
    FSTP DWORD PTR [SUM]   ; SUM = сума

    ; Завершення програми
    mov ah, 4Ch
    int 21h

END START
