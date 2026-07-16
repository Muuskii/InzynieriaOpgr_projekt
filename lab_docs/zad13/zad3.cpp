#include <iostream>

class A {
public:
    int msi_publicLiczba;
protected:
    int mi_protLiczba;
private:
    int mi_privLiczba;

public:
    A() {
        msi_publicLiczba = 0;
        mi_protLiczba = 0;
        mi_privLiczba = 0;
    }

    int pokazLiczbe() {
        std::cout << "Publiczna: " << msi_publicLiczba << std::endl;
        std::cout << "Chroniona: " << mi_protLiczba << std::endl;
        std::cout << "Prywatna:  " << mi_privLiczba << std::endl;
        return 0; 
    }
	
    void przypiszWartosc(int pub, int prot, int priv) {
        msi_publicLiczba = pub;
        mi_protLiczba = prot;
        mi_privLiczba = priv;
    }
};

int main() {
    
    A obiekt;

    std::cout << "Wartosci poczatkowe" << std::endl;
    obiekt.pokazLiczbe();
	
    obiekt.przypiszWartosc(10, 20, 30);

    std::cout << "Po zmianie wartosci" << std::endl;
    obiekt.pokazLiczbe();

    return 0;
}