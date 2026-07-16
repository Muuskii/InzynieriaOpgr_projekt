#include <iostream>

// Główna klasa bazowa
class A {
public:
    int wartoscA;
    A() : wartoscA(100) {} // Konstruktor domyślny klasy A
};

// Klasa B dziedziczy wirtualnie po A
class B : virtual public A {
public:
    void wyswietlB() {
        std::cout << "Metoda z klasy B (dostęp do wartoscA: " << wartoscA << ")" << std::endl;
    }
};

// Klasa C dziedziczy wirtualnie po A
class C : virtual public A {
public:
    void wyswietlC() {
        std::cout << "Metoda z klasy C (dostęp do wartoscA: " << wartoscA << ")" << std::endl;
    }
};

// Klasa D dziedziczy po B oraz C
class D : public B, public C {
public:
    void pokazWspolnaWartosc() {
        std::cout << "Wartosc pola z klasy A wywolana z poziomu klasy D: " << wartoscA << std::endl;
    }
};

int main() {
    D obiektD;

    std::cout << "Test dziedziczenia" << std::endl;
    
    // Wywołanie metod z klas pośrednich
    obiektD.wyswietlB();
    obiektD.wyswietlC();
    
    // Bezpośredni i bezproblemowy dostęp do pola klasy najwyższej w hierarchii
    obiektD.pokazWspolnaWartosc();

    // Możemy też bezpośrednio zmodyfikować tę wartość
    obiektD.wartoscA = 500;
    std::cout << "Po modyfikacji pola wartoscA w obiekcie D:" << std::endl;
    obiektD.pokazWspolnaWartosc();

    return 0;
}