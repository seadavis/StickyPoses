using System;

class Taschenrechner {

    static void Main() {

        while(true) {
            Console.Write("Zahl 1: ");
            float zahl1 = float.Parse(Console.ReadLine());  // 1. Zahl einlesen

            Console.Write("Zahl 2: ");
            float zahl2 = float.Parse(Console.ReadLine());  // 2. Zahl einlesen

            Console.Write("Zeichen: ");
            char zeichen = char.Parse(Console.ReadLine());  // Rechenzeichen einlesen

            float ergebnis = 0f;

            if(zeichen == '+') {                            // Rechenoperation auswählen
                ergebnis = Addieren(zahl1, zahl2);
            } else if(zeichen == '-') {
                ergebnis = Subtrahieren(zahl1, zahl2);
            } else if(zeichen == '*') {
                ergebnis = Multiplizieren(zahl1, zahl2);
            } else if(zeichen == '/') {
                ergebnis = Dividieren(zahl1, zahl2);
            } else {
                Console.WriteLine("Ungültiges Rechenzeichen!");
            }

            if(ergebnis != float.MaxValue) {
                Console.WriteLine(ergebnis);                // Ergebnis ausgeben
            } else {
                Console.WriteLine("Dividieren nicht möglich!");
            }
        }
    }

    static float Addieren(float a, float b) {
        return a + b;
    }

    static float Subtrahieren(float a, float b) {
        return a - b;
    }

    static float Multiplizieren(float a, float b) {
        return a * b;
    }

    static float Dividieren(float a, float b) {
        if(b > 0) {
            return a / b;
        } else {
            return float.MaxValue;
        }
    }

    static void Test() {
        
    }
}