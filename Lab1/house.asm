MODEL SMALL
STACK 256
DATASEG
CODESEG

start:
    ; Встановлення графічного режиму 320x200, 256 кольорів
    mov ax, 0013h
    int 10h

    ; Малюємо стіни будинку (прямокутник)
    mov cx, 80         ; Початкова X-координата (лівий верхній кут)
    mov dx, 90         ; Початкова Y-координата (лівий верхній кут)
    mov al, 15         ; Колір: білий (15)
    mov ah, 0Ch        ; Функція малювання пікселя

    ; Вертикальні лінії (стіни)
    mov bx, 80         ; Висота стін
wall_v:
    mov cx, 80         ; Ліва стіна (X=80)
    call draw_pixel
    mov cx, 180        ; Права стіна (X=180)
    call draw_pixel
    inc dx             ; Наступний рядок
    dec bx
    jnz wall_v

    ; Горизонтальні лінії (основа та верх)
    mov dx, 90         ; Верхня лінія (Y=90)
    mov bx, 100        ; Ширина будинку
wall_h1:
    mov cx, 80         ; Початок X
    add cx, bx
    call draw_pixel
    dec bx
    jnz wall_h1

    mov dx, 170        ; Нижня лінія (Y=170)
    mov bx, 100
wall_h2:
    mov cx, 80
    add cx, bx
    call draw_pixel
    dec bx
    jnz wall_h2

    ; Малюємо дах (трикутник)
    mov dx, 30         ; Початок з вершини даху (Y=30)
    mov bx, 0          ; Лічильник для ширини трикутника
roof:
    mov cx, 130        ; Центр даху (X=130)
    sub cx, bx         ; Ліва частина трикутника
    call draw_pixel
    mov cx, 130
    add cx, bx         ; Права частина трикутника
    call draw_pixel
    inc bx             ; Збільшуємо ширину
    inc dx             ; Спускаємося вниз по Y
    cmp dx, 90         ; Основа даху (Y=90)
    jne roof

    ; Малюємо двері (прямокутник)
    mov al, 6          ; Колір: коричневий (6)
    mov dx, 140        ; Початок дверей (Y=140)
    mov bx, 30         ; Висота дверей
door_v:
    mov cx, 115        ; Ліва сторона дверей (X=115)
door_h:
    call draw_pixel
    inc cx             ; Рухаємося праворуч
    cmp cx, 145        ; Права сторона дверей (X=145)
    jne door_h
    inc dx             ; Наступний рядок
    dec bx
    jnz door_v

    ; Малюємо вікно (прямокутник)
    mov al, 9          ; Колір: блакитний (9)
    mov dx, 110        ; Початок вікна (Y=110)
    mov bx, 20         ; Висота вікна
window_v:
    mov cx, 90         ; Ліва сторона вікна (X=90)
window_h:
    call draw_pixel
    inc cx             ; Рухаємося праворуч
    cmp cx, 110        ; Права сторона вікна (X=110)
    jne window_h
    inc dx             ; Наступний рядок
    dec bx
    jnz window_v

    ; Очікування натискання клавіші
ex:
    mov ah, 1
    int 16h
    jz ex              ; Чекаємо, поки не буде натиснуто клавішу

    ; Повернення в текстовий режим
    mov ax, 0003h
    int 10h

    ; Вихід з програми
    mov ah, 04Ch
    int 21h

; Процедура малювання пікселя
draw_pixel:
    mov ah, 0Ch
    int 10h
    ret

END start