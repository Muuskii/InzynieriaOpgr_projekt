#include <iostream>


class A {
public:
    void fA() {
        std::cout << "Wywolano fA() z obiektu klasy A." << std::endl;
    }
};

class B {
private:
    A* pa;

public:
    
    B(A* zewnetrznyA) : pa(zewnetrznyA) {}

    void fB() {
        std::cout << "Wywolano fB() z obiektu klasy B. Korzystam z A:" << std::endl;
        if (pa != nullptr) {
            pa->fA();
        }
    }
};


class Punkt {
public:
    Punkt() {
        std::cout << "Utworzono Punkt (Srodek Kola)." << std::endl;
    }
    ~Punkt() {
        std::cout << "Zniszczono Punkt." << std::endl;
    }
};

class Kolo {
private:
    double dPromienKola;
    Punkt SrodekKola; 

public:

    Kolo(double promien) : dPromienKola(promien), SrodekKola() {
        std::cout << "Utworzono Kolo o promieniu: " << dPromienKola << std::endl;
    }

   
    ~Kolo() {
        std::cout << "Zniszczono Kolo." << std::endl;
       
    }

    void WstawSrodek() {
        std::cout << "Wywolano WstawSrodek() dla Kola." << std::endl;
    }
};


int main() {
    std::cout << "TEST AGREGACJI" << std::endl;
    A* obiektA = new A();
    B obiektB(obiektA);   
    obiektB.fB();

    delete obiektA;
    std::cout << "Obiekt A zostal usuniety na zewnatrz." << std::endl;

    std::cout << "TEST KOMPOZYCJI" << std::endl;
    {
        Kolo mojeKolo(5.5);
        mojeKolo.WstawSrodek();
    }

    return 0;
}