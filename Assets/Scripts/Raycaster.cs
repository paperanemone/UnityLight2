#define _CRT_SECURE_NO_WARNINGS 1
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>
#include <locale.h>

// Структура для хранения информации о прокате велосипедов
typedef struct {
    int bike_number;           // Номер велосипеда
    char full_name[100];        // ФИО арендатора
    char rental_date[11];       // Дата аренды (ДД.ММ.ГГГГ)
    int rental_period;          // Срок аренды (в днях)
    float rental_cost;          // Стоимость аренды
} Rental;

// Глобальный массив для хранения прокатов
Rental* rental_array = NULL;
int rental_count = 0;
const char* FILENAME = "rentals.txt";

// Прототипы функций
void load_from_file();
void save_to_file();
void add_new_rental();
void delete_rental();
void print_rental(Rental r);
void print_all_rentals();
void linear_search_by_name(const char* name);
void binary_search_by_bike_number(int bike_number);
void quick_sort_by_date(Rental arr[], int low, int high);
void selection_sort_by_period();
void insertion_sort_by_name();
void search_by_date_and_period(const char* date, int period);
void find_most_popular_bike();
void free_memory();
void clear_screen();
void wait_for_keypress();

// Вспомогательные функции
int partition_by_date(Rental arr[], int low, int high);
void swap(Rental* a, Rental* b);
int compare_dates(const char* date1, const char* date2);

int main() {
    setbuf(stdout, NULL);
    setlocale(LC_ALL, "Rus");

    printf("\n==================================================\n");
    printf("     СИСТЕМА УЧЕТА ПРОКАТА ВЕЛОСИПЕДОВ\n");
    printf("==================================================\n");
    printf("Работа с текстовым файлом: %s\n", FILENAME);
    printf("==================================================\n\n");

    // Загружаем данные из файла
    load_from_file();

    int choice;
    do {
        printf("\n--------------------------------------------------\n");
        printf("  1. Показать все прокаты\n");
        printf("  2. Добавить новый прокат\n");
        printf("  3. Удалить прокат\n");
        printf("  4. Линейный поиск по ФИО арендатора\n");
        printf("  5. Бинарный поиск по номеру велосипеда\n");
        printf("  6. Быстрая сортировка по дате аренды\n");
        printf("  7. Сортировка выбором по сроку аренды\n");
        printf("  8. Сортировка вставками по ФИО арендатора\n");
        printf("  9. Поиск по дате и сроку аренды\n");
        printf(" 10. Статистика: самый популярный велосипед\n");
        printf(" 11. Сохранить изменения в файл\n");
        printf("  0. Выход\n");
        printf("--------------------------------------------------\n");
        printf("Выберите действие: ");

        if (scanf("%d", &choice) != 1) {
            while (getchar() != '\n');
            printf("❌ Неверный ввод! Введите число.\n");
            wait_for_keypress();
            continue;
        }
        getchar();

        switch (choice) {
        case 1:
            print_all_rentals();
            wait_for_keypress();
            break;
        case 2:
            add_new_rental();
            wait_for_keypress();
            break;
        case 3:
            delete_rental();
            wait_for_keypress();
            break;
        case 4: {
            char search_name[100];
            printf("Введите ФИО для поиска: ");
            fgets(search_name, sizeof(search_name), stdin);
            search_name[strcspn(search_name, "\n")] = 0;
            linear_search_by_name(search_name);
            wait_for_keypress();
            break;
        }
        case 5: {
            int search_bike;
            printf("Введите номер велосипеда для поиска: ");
            scanf("%d", &search_bike);
            getchar();
            binary_search_by_bike_number(search_bike);
            wait_for_keypress();
            break;
        }
        case 6:
            if (rental_count > 0) {
                quick_sort_by_date(rental_array, 0, rental_count - 1);
                printf("\n✓ Массив отсортирован по дате аренды (быстрая сортировка)\n");
                print_all_rentals();
            }
            else {
                printf("\n❌ Нет данных для сортировки!\n");
            }
            wait_for_keypress();
            break;
        case 7:
            if (rental_count > 0) {
                selection_sort_by_period();
                printf("\n✓ Массив отсортирован по сроку аренды (сортировка выбором)\n");
                print_all_rentals();
            }
            else {
                printf("\n❌ Нет данных для сортировки!\n");
            }
            wait_for_keypress();
            break;
        case 8:
            if (rental_count > 0) {
                insertion_sort_by_name();
                printf("\n✓ Массив отсортирован по ФИО арендатора (сортировка вставками)\n");
                print_all_rentals();
            }
            else {
                printf("\n❌ Нет данных для сортировки!\n");
            }
            wait_for_keypress();
            break;
        case 9: {
            char date[11];
            int period;
            printf("Введите дату для поиска (ДД.ММ.ГГГГ): ");
            fgets(date, sizeof(date), stdin);
            date[strcspn(date, "\n")] = 0;
            printf("Введите минимальный срок аренды (дней): ");
            scanf("%d", &period);
            getchar();
            search_by_date_and_period(date, period);
            wait_for_keypress();
            break;
        }
        case 10:
            find_most_popular_bike();
            wait_for_keypress();
            break;
        case 11:
            save_to_file();
            wait_for_keypress();
            break;
        }
    } while (choice != 0);

    // Спрашиваем, сохранять ли изменения при выходе
    if (rental_count > 0 && rental_array != NULL) {
        char save_choice;
        printf("\nСохранить изменения перед выходом? (y/n): ");
        scanf("%c", &save_choice);
        getchar();
        if (save_choice == 'y' || save_choice == 'Y') {
            save_to_file();
        }
    }

    free_memory();
    printf("\nДо свидания!\n");
    return 0;
}

// Загрузка данных из текстового файла
void load_from_file() {
    FILE* file = fopen(FILENAME, "r");

    if (file == NULL) {
        printf("\n❌ ОШИБКА: Невозможно открыть файл %s\n", FILENAME);
        printf("   Убедитесь, что файл существует в текущей директории.\n");
        printf("   Файл должен быть в формате:\n");
        printf("   номер велосипеда\n");
        printf("   ФИО арендатора\n");
        printf("   дата аренды (ДД.ММ.ГГГГ)\n");
        printf("   срок аренды (дней)\n");
        printf("   стоимость аренды\n");
        rental_count = 0;
        rental_array = NULL;
        return;
    }

    printf("\n✓ Файл %s успешно открыт для чтения\n", FILENAME);

    // Сначала подсчитываем количество записей
    char buffer[256];
    int count = 0;

    while (fgets(buffer, sizeof(buffer), file)) {
        count++;
    }

    rental_count = count / 5;

    if (rental_count == 0) {
        printf("❌ Файл не содержит данных!\n");
        fclose(file);
        rental_array = NULL;
        return;
    }

    // Возвращаемся в начало файла
    rewind(file);

    // Выделяем память
    rental_array = (Rental*)malloc(rental_count * sizeof(Rental));
    if (rental_array == NULL) {
        printf("❌ Ошибка выделения памяти!\n");
        fclose(file);
        rental_count = 0;
        return;
    }

    // Читаем данные
    for (int i = 0; i < rental_count; i++) {
        // Читаем номер велосипеда
        fgets(buffer, sizeof(buffer), file);
        rental_array[i].bike_number = atoi(buffer);

        // Читаем ФИО
        fgets(rental_array[i].full_name, sizeof(rental_array[i].full_name), file);
        rental_array[i].full_name[strcspn(rental_array[i].full_name, "\n")] = 0;

        // Читаем дату
        fgets(rental_array[i].rental_date, sizeof(rental_array[i].rental_date), file);
        rental_array[i].rental_date[strcspn(rental_array[i].rental_date, "\n")] = 0;

        // Читаем срок аренды
        fgets(buffer, sizeof(buffer), file);
        rental_array[i].rental_period = atoi(buffer);

        // Читаем стоимость
        fgets(buffer, sizeof(buffer), file);
        rental_array[i].rental_cost = atof(buffer);
    }

    fclose(file);
    printf("✓ Данные успешно загружены\n");
    printf("  Записей: %d\n", rental_count);
}

// Сохранение данных в текстовый файл
void save_to_file() {
    if (rental_count == 0 || rental_array == NULL) {
        printf("\n❌ Нет данных для сохранения!\n");
        return;
    }

    FILE* file = fopen(FILENAME, "w");

    if (file == NULL) {
        printf("\n❌ ОШИБКА: Невозможно создать файл %s для записи\n", FILENAME);
        return;
    }

    printf("\n✓ Файл %s успешно создан для записи\n", FILENAME);

    // Записываем данные
    for (int i = 0; i < rental_count; i++) {
        fprintf(file, "%d\n", rental_array[i].bike_number);
        fprintf(file, "%s\n", rental_array[i].full_name);
        fprintf(file, "%s\n", rental_array[i].rental_date);
        fprintf(file, "%d\n", rental_array[i].rental_period);
        fprintf(file, "%.2f\n", rental_array[i].rental_cost);
    }

    fclose(file);
    printf("✓ Данные успешно сохранены\n");
    printf("  Записей: %d\n", rental_count);
}

// Добавление нового проката
void add_new_rental() {
    // Если массив пуст, создаем новый
    if (rental_array == NULL) {
        rental_count = 0;
        rental_array = (Rental*)malloc(sizeof(Rental));
        if (rental_array == NULL) {
            printf("❌ Ошибка выделения памяти!\n");
            return;
        }
    }
    else {
        // Расширяем существующий массив
        Rental* temp = (Rental*)realloc(rental_array, (rental_count + 1) * sizeof(Rental));
        if (temp == NULL) {
            printf("❌ Ошибка выделения памяти!\n");
            return;
        }
        rental_array = temp;
    }

    Rental* new_rental = &rental_array[rental_count];

    printf("\n========================================\n");
    printf("     ДОБАВЛЕНИЕ НОВОГО ПРОКАТА\n");
    printf("========================================\n");

    printf("Номер велосипеда: ");
    scanf("%d", &new_rental->bike_number);
    getchar();

    printf("ФИО арендатора: ");
    fgets(new_rental->full_name, 100, stdin);
    new_rental->full_name[strcspn(new_rental->full_name, "\n")] = 0;

    // Проверка на пустой ввод
    if (strlen(new_rental->full_name) == 0) {
        printf("Введите ФИО: ");
        fgets(new_rental->full_name, 100, stdin);
        new_rental->full_name[strcspn(new_rental->full_name, "\n")] = 0;
    }

    printf("Дата аренды (ДД.ММ.ГГГГ): ");
    fgets(new_rental->rental_date, 11, stdin);
    new_rental->rental_date[strcspn(new_rental->rental_date, "\n")] = 0;

    printf("Срок аренды (дней): ");
    scanf("%d", &new_rental->rental_period);
    getchar();

    printf("Стоимость аренды: ");
    scanf("%f", &new_rental->rental_cost);
    getchar();

    rental_count++;
    printf("\n✓ Новый прокат успешно добавлен!\n");
    printf("  Всего записей: %d\n", rental_count);
}

// Удаление проката
void delete_rental() {
    if (rental_count == 0 || rental_array == NULL) {
        printf("\n❌ Нет записей для удаления.\n");
        return;
    }

    print_all_rentals();

    int index;
    printf("Введите номер проката для удаления (1-%d): ", rental_count);
    scanf("%d", &index);
    getchar();

    if (index < 1 || index > rental_count) {
        printf("❌ Неверный номер!\n");
        return;
    }

    index--;

    printf("\n✓ Удален прокат: %s (велосипед №%d)\n", 
           rental_array[index].full_name, rental_array[index].bike_number);

    // Сдвигаем элементы
    for (int i = index; i < rental_count - 1; i++) {
        rental_array[i] = rental_array[i + 1];
    }

    rental_count--;

    if (rental_count > 0) {
        Rental* temp = (Rental*)realloc(rental_array, rental_count * sizeof(Rental));
        if (temp != NULL) {
            rental_array = temp;
        }
    }
    else {
        free(rental_array);
        rental_array = NULL;
    }

    printf("✓ Прокат успешно удален!\n");
    printf("  Осталось записей: %d\n", rental_count);
}

// Вывод информации о прокате
void print_rental(Rental r) {
    printf("  Номер велосипеда: %d\n", r.bike_number);
    printf("    ФИО: %s\n", r.full_name);
    printf("    Дата аренды: %s\n", r.rental_date);
    printf("    Срок аренды: %d дней\n", r.rental_period);
    printf("    Стоимость: %.2f руб\n", r.rental_cost);
    printf("\n");
}

// Вывод всех прокатов
void print_all_rentals() {
    if (rental_count == 0 || rental_array == NULL) {
        printf("\n❌ Нет данных о прокатах.\n");
        return;
    }

    printf("\n============================================================\n");
    printf("                     ВСЕ ПРОКАТЫ\n");
    printf("============================================================\n");
    printf("  Всего записей: %d\n", rental_count);
    printf("  Файл: %s\n", FILENAME);
    printf("------------------------------------------------------------\n");

    for (int i = 0; i < rental_count; i++) {
        printf("[%d] ", i + 1);
        print_rental(rental_array[i]);
    }
}

// Линейный поиск по ФИО арендатора
void linear_search_by_name(const char* name) {
    if (rental_count == 0 || rental_array == NULL) {
        printf("\n❌ Нет данных для поиска.\n");
        return;
    }

    printf("\n==================================================\n");
    printf("   ЛИНЕЙНЫЙ ПОИСК: \"%s\"\n", name);
    printf("==================================================\n");

    bool found = false;
    int count = 0;

    for (int i = 0; i < rental_count; i++) {
        if (strstr(rental_array[i].full_name, name) != NULL) {
            if (!found) {
                printf("✅ Найденные прокаты:\n\n");
                found = true;
            }
            printf("[%d] ", i + 1);
            print_rental(rental_array[i]);
            count++;
        }
    }

    if (!found) {
        printf("❌ Прокаты не найдены\n");
    }
    else {
        printf("  Всего найдено: %d\n", count);
    }
}

// Бинарный поиск по номеру велосипеда
void binary_search_by_bike_number(int bike_number) {
    if (rental_count == 0 || rental_array == NULL) {
        printf("\n❌ Нет данных для поиска.\n");
        return;
    }

    printf("\n==================================================\n");
    printf("   БИНАРНЫЙ ПОИСК: номер велосипеда = %d\n", bike_number);
    printf("==================================================\n");

    // Создаем копию и сортируем по номеру велосипеда
    Rental* temp = (Rental*)malloc(rental_count * sizeof(Rental));
    if (temp == NULL) {
        printf("❌ Ошибка выделения памяти!\n");
        return;
    }

    memcpy(temp, rental_array, rental_count * sizeof(Rental));
    
    // Сортируем по номеру велосипеда
    for (int i = 0; i < rental_count - 1; i++) {
        for (int j = 0; j < rental_count - i - 1; j++) {
            if (temp[j].bike_number > temp[j + 1].bike_number) {
                swap(&temp[j], &temp[j + 1]);
            }
        }
    }

    int left = 0, right = rental_count - 1;
    bool found = false;

    while (left <= right) {
        int mid = left + (right - left) / 2;

        if (temp[mid].bike_number == bike_number) {
            printf("✅ Найден прокат:\n");
            print_rental(temp[mid]);
            found = true;
            break;
        }

        if (temp[mid].bike_number < bike_number) {
            left = mid + 1;
        }
        else {
            right = mid - 1;
        }
    }

    if (!found) {
        printf("❌ Прокат с номером велосипеда %d не найден\n", bike_number);
    }

    free(temp);
}

// Быстрая сортировка по дате аренды
int partition_by_date(Rental arr[], int low, int high) {
    char* pivot = arr[high].rental_date;
    int i = (low - 1);

    for (int j = low; j <= high - 1; j++) {
        if (compare_dates(arr[j].rental_date, pivot) < 0) {
            i++;
            swap(&arr[i], &arr[j]);
        }
    }
    swap(&arr[i + 1], &arr[high]);
    return (i + 1);
}

void quick_sort_by_date(Rental arr[], int low, int high) {
    if (low < high) {
        int pi = partition_by_date(arr, low, high);
        quick_sort_by_date(arr, low, pi - 1);
        quick_sort_by_date(arr, pi + 1, high);
    }
}

// Сортировка выбором по сроку аренды
void selection_sort_by_period() {
    if (rental_count == 0 || rental_array == NULL) {
        return;
    }

    for (int i = 0; i < rental_count - 1; i++) {
        int min_idx = i;
        for (int j = i + 1; j < rental_count; j++) {
            if (rental_array[j].rental_period < rental_array[min_idx].rental_period) {
                min_idx = j;
            }
        }
        if (min_idx != i) {
            swap(&rental_array[i], &rental_array[min_idx]);
        }
    }
}

// Сортировка вставками по ФИО арендатора
void insertion_sort_by_name() {
    if (rental_count == 0 || rental_array == NULL) {
        return;
    }

    for (int i = 1; i < rental_count; i++) {
        Rental key = rental_array[i];
        int j = i - 1;

        while (j >= 0 && strcmp(rental_array[j].full_name, key.full_name) > 0) {
            rental_array[j + 1] = rental_array[j];
            j--;
        }
        rental_array[j + 1] = key;
    }
}

// Поиск велосипедов по дате и сроку аренды
void search_by_date_and_period(const char* date, int period) {
    if (rental_count == 0 || rental_array == NULL) {
        printf("\n❌ Нет данных для поиска.\n");
        return;
    }

    printf("\n==================================================\n");
    printf("   ПОИСК: после даты %s, срок >= %d дней\n", date, period);
    printf("==================================================\n");

    bool found = false;
    int count = 0;

    for (int i = 0; i < rental_count; i++) {
        if (compare_dates(rental_array[i].rental_date, date) > 0 && 
            rental_array[i].rental_period >= period) {
            if (!found) {
                printf("✅ Найденные прокаты:\n\n");
                found = true;
            }
            printf("[%d] Велосипед №%d\n", ++count, rental_array[i].bike_number);
            printf("     Арендатор: %s\n", rental_array[i].full_name);
            printf("     Дата: %s, Срок: %d дней, Стоимость: %.2f руб\n",
                   rental_array[i].rental_date, 
                   rental_array[i].rental_period,
                   rental_array[i].rental_cost);
        }
    }

    if (!found) {
        printf("❌ Прокаты не найдены\n");
    }
}

// Статистика: самый популярный велосипед
void find_most_popular_bike() {
    if (rental_count == 0 || rental_array == NULL) {
        printf("\n❌ Нет данных для статистики.\n");
        return;
    }

    printf("\n==================================================\n");
    printf("     СТАТИСТИКА: САМЫЙ ПОПУЛЯРНЫЙ ВЕЛОСИПЕД\n");
    printf("==================================================\n");

    // Создаем массив для подсчета количества аренд каждого велосипеда
    int* bike_counts = (int*)calloc(rental_count, sizeof(int));
    int* unique_bikes = (int*)malloc(rental_count * sizeof(int));
    int unique_count = 0;

    if (bike_counts == NULL || unique_bikes == NULL) {
        printf("❌ Ошибка выделения памяти!\n");
        free(bike_counts);
        free(unique_bikes);
        return;
    }

    // Подсчитываем количество аренд для каждого велосипеда
    for (int i = 0; i < rental_count; i++) {
        int bike_num = rental_array[i].bike_number;
        bool found = false;
        
        for (int j = 0; j < unique_count; j++) {
            if (unique_bikes[j] == bike_num) {
                bike_counts[j]++;
                found = true;
                break;
            }
        }
        
        if (!found) {
            unique_bikes[unique_count] = bike_num;
            bike_counts[unique_count] = 1;
            unique_count++;
        }
    }

    // Находим максимальное количество аренд
    int max_count = 0;
    for (int i = 0; i < unique_count; i++) {
        if (bike_counts[i] > max_count) {
            max_count = bike_counts[i];
        }
    }

    // Выводим все велосипеды с максимальным количеством аренд
    printf("Самый популярный велосипед (арендован %d раз):\n", max_count);
    printf("----------------------------------------\n");
    
    bool found = false;
    for (int i = 0; i < unique_count; i++) {
        if (bike_counts[i] == max_count) {
            printf("  ✅ Велосипед №%d\n", unique_bikes[i]);
            found = true;
        }
    }

    if (!found) {
        printf("❌ Данные не найдены\n");
    }

    free(bike_counts);
    free(unique_bikes);
}

// Вспомогательные функции
void swap(Rental* a, Rental* b) {
    Rental temp = *a;
    *a = *b;
    *b = temp;
}

// Сравнение дат в формате ДД.ММ.ГГГГ
int compare_dates(const char* date1, const char* date2) {
    int d1, m1, y1, d2, m2, y2;
    sscanf(date1, "%d.%d.%d", &d1, &m1, &y1);
    sscanf(date2, "%d.%d.%d", &d2, &m2, &y2);
    
    if (y1 != y2) return y1 - y2;
    if (m1 != m2) return m1 - m2;
    return d1 - d2;
}

void clear_screen() {
#ifdef _WIN32
    system("cls");
#else
    system("clear");
#endif
}

void wait_for_keypress() {
    printf("\nНажмите Enter для продолжения...");
    getchar();
}

void free_memory() {
    if (rental_array != NULL) {
        free(rental_array);
        rental_array = NULL;
    }
}