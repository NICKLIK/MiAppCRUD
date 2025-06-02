import { useEffect, useState } from "react";
import jsPDF from "jspdf";
import "./Productos.css";

function HistorialVentas() {
    const [ventas, setVentas] = useState([]);
    const [ventasFiltradas, setVentasFiltradas] = useState([]);
    const [productoFiltro, setProductoFiltro] = useState("");
    const [fechaFiltro, setFechaFiltro] = useState("");
    const [ventaSeleccionada, setVentaSeleccionada] = useState(null);

    useEffect(() => {
        const historialGlobal = [];

        for (let i = 0; i < localStorage.length; i++) {
            const key = localStorage.key(i);
            if (key.startsWith("historialCompras_")) {
                const correo = key.replace("historialCompras_", "");
                const compras = JSON.parse(localStorage.getItem(key));
                const perfil = JSON.parse(localStorage.getItem(`perfilUsuario_${correo}`)) || {};

                compras.forEach((compra) => {
                    historialGlobal.push({ ...compra, correo, perfil });
                });
            }
        }

        setVentas(historialGlobal);
        setVentasFiltradas(historialGlobal);
    }, []);

    const aplicarFiltros = () => {
        const filtradas = ventas.filter((venta) => {
            let coincideFecha = true;

            if (fechaFiltro !== "") {
                try {
                    // Extraer solo la parte de la fecha (antes de la coma)
                    const [dia, mes, anio] = venta.fecha.split(",")[0].split("/").map(p => p.trim());
                    const fechaVentaFormateada = `${anio}-${mes.padStart(2, '0')}-${dia.padStart(2, '0')}`;

                    coincideFecha = fechaVentaFormateada === fechaFiltro;
                } catch (error) {
                    console.error("Error al procesar la fecha:", venta.fecha, error);
                    coincideFecha = false;
                }
            }

            const coincideProducto =
                productoFiltro === "" ||
                venta.productos.some((p) =>
                    p.nombre.toLowerCase().includes(productoFiltro.toLowerCase())
                );

            return coincideFecha && coincideProducto;
        });

        setVentasFiltradas(filtradas);
    };


    const generarPDF = () => {
        const doc = new jsPDF();
        doc.setFontSize(14);
        doc.text("Detalles de Ventas Filtradas", 20, 20);
        let y = 30;

        ventasFiltradas.forEach((venta, index) => {
            doc.setFontSize(12);
            doc.text(`Compra #${index + 1}`, 20, y); y += 7;
            doc.text(`Fecha: ${venta.fecha}`, 20, y); y += 7;
            doc.text(`Usuario: ${venta.perfil.nombre || ""} ${venta.perfil.apellido || ""} (${venta.correo})`, 20, y); y += 7;
            doc.text(`Total: $${venta.totalDinero} | ${venta.totalPuntos} EcuniPoints`, 20, y); y += 7;
            doc.text(`Metodo: ${venta.metodoPago}`, 20, y); y += 7;
            doc.text("Productos:", 20, y); y += 6;
            venta.productos.forEach(p => {
                doc.text(` - ${p.nombre} (${p.cantidad} unidades)`, 25, y); y += 6;
            });
            y += 5;
            if (y > 270) { doc.addPage(); y = 20; }
        });

        doc.save("Ventas_Filtradas.pdf");
    };

    return (
        <div className="productos-container">
            <h1>Historial de Ventas</h1>

            <div style={{ display: "flex", gap: "1rem", justifyContent: "center", marginBottom: "1.5rem" }}>
                <input
                    type="text"
                    placeholder="Buscar por producto..."
                    value={productoFiltro}
                    onChange={(e) => setProductoFiltro(e.target.value)}
                />
                <input
                    type="date"
                    value={fechaFiltro}
                    onChange={(e) => setFechaFiltro(e.target.value)}
                />
                <button onClick={aplicarFiltros}>Buscar</button>
                <button onClick={generarPDF}>Descargar Detalles</button>
            </div>

            {ventasFiltradas.length === 0 ? (
                <p style={{ textAlign: "center" }}>No se encontraron ventas para los filtros seleccionados.</p>
            ) : (
                <div className="productos-list">
                    {ventasFiltradas.map((venta, i) => (
                        <div key={i} className="producto-card">
                            <h3>Compra #{i + 1}</h3>
                            <p><strong>Fecha:</strong> {venta.fecha}</p>
                            <p><strong>Método de Pago:</strong> {venta.metodoPago}</p>
                            <button onClick={() => setVentaSeleccionada(venta)}>Revisar Compra</button>
                        </div>
                    ))}
                </div>
            )}

            {ventaSeleccionada && (
                <div className="modal-overlay" onClick={() => setVentaSeleccionada(null)}>
                    <div className="modal-contenido" onClick={(e) => e.stopPropagation()}>
                        <h2 style={{ color: "green" }}>Resumen de la Compra</h2>
                        {ventaSeleccionada.productos.map((p, idx) => (
                            <p key={idx}><strong>{p.nombre}</strong> - {p.cantidad} unidades</p>
                        ))}
                        <p><strong>Total en Dinero:</strong> ${parseFloat(ventaSeleccionada.totalDinero).toFixed(2)}</p>
                        <p><strong>Total en EcuniPoints:</strong> {ventaSeleccionada.totalPuntos}</p>
                        <p><strong>Método de Pago:</strong> {ventaSeleccionada.metodoPago}</p>
                        {ventaSeleccionada.metodoPago === "dinero" ? (
                            <p>Tu compra fue hecha con dinero, así que recibirás <strong>{ventaSeleccionada.ecuniPointsGanados} EcuniPoints</strong>.</p>
                        ) : (
                            <p>Tu compra fue realizada con EcuniPoints, por lo que <strong>no recibirás recompensa</strong>.</p>
                        )}
                        <div className="modal-botones">
                            <button onClick={() => setVentaSeleccionada(null)}>Aceptar</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

export default HistorialVentas;
