#include <iostream>

// Klasa bazowa A
class A {
public:
    void funkcjaA() {
        std::cout << "Wywolano: funkcjaA() z klasy bazowej A" << std::endl;
    }
};

// Klasa pochodna B dziedziczaca publicznie po A
class B : public A {
public:
    void funkcjaB() {
        std::cout << "Wywolano: funkcjaB() z klasy pochodnej B" << std::endl;
    }
};

int main() {
    std::cout << "Test obiektu klasy bazowej A" << std::endl;
    A obiektA;
    obiektA.funkcjaA(); // Obiekt klasy A może wywołać tylko swoją metodę
    // obiektA.funkcjaB(); // BLAD KOMPILACJI! Klasa A nie zna funkcji B.

    std::cout << "Test obiektu klasy pochodnej B" << std::endl;
    B obiektB;
    
    // Wywołanie metody klasy B
    obiektB.funkcjaB();
    
    // Wywołanie metody klasy A na rzecz obiektu klasy B
    obiektB.funkcjaA(); 

    return 0;
}