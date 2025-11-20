namespace SupermercadoBackend.DTOs
{
    public class CrearPedidoDto
    {
        public List<DetallePedidoDto> Detalles { get; set; } = new List<DetallePedidoDto>();
    }

    public class DetallePedidoDto
    {
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}