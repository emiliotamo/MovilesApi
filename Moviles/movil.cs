namespace Moviles
{
    public class Movil
    {
        public Movil(string marca, decimal precio, int capacidad)
        {
            Marca = marca;
            Precio = precio;
            Capacidad = capacidad;
        }

        public string Marca { get; set; }
        public decimal Precio { get; set; }
        public int Capacidad { get; set; } // Capacidad en GB
    }
}
