import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import jsPDF from "jspdf";
import "./Productos.css";

function HistorialCompras() {
    const correoUsuario = localStorage.getItem("usuario");
    const navigate = useNavigate();
    const [historial, setHistorial] = useState([]);
    const [compraSeleccionada, setCompraSeleccionada] = useState(null);

    useEffect(() => {
        const data = JSON.parse(localStorage.getItem(`historialCompras_${correoUsuario}`)) || [];
        setHistorial(data);
    }, [correoUsuario]);


    const generarPDF = () => {
        const doc = new jsPDF();
        doc.setFontSize(14);
        const compra = compraSeleccionada;
        let y = 20;

        
        doc.text("Resumen de la Compra", 20, y);
        y += 10;

        
        const perfil = JSON.parse(localStorage.getItem(`perfilUsuario_${correoUsuario}`));
        if (perfil) {
            doc.setFontSize(12);
            doc.text(`Nombre: ${perfil.nombre} ${perfil.apellido}`, 20, y); y += 8;
            doc.text(`Cédula: ${perfil.cedula}`, 20, y); y += 8;
            doc.text(`Provincia: ${perfil.provincia}`, 20, y); y += 8;
            doc.text(`Ciudad: ${perfil.ciudad}`, 20, y); y += 10;
        }

       
        doc.setFontSize(12);
        doc.text(`Fecha: ${compra.fecha}`, 20, y); y += 8;
        doc.text(`Método de Pago: ${compra.metodoPago}`, 20, y); y += 8;
        doc.text(`Total en Dinero: $${compra.totalDinero}`, 20, y); y += 8;
        doc.text(`Total en EcuniPoints: ${compra.totalPuntos}`, 20, y); y += 8;

        if (compra.metodoPago === "dinero") {
            doc.text(`EcuniPoints ganados: ${compra.ecuniPointsGanados}`, 20, y);
            y += 10;
        }

        doc.setFont(undefined, "bold");
        doc.text("Productos:", 20, y);
        doc.setFont(undefined, "normal");
        y += 8;

        compra.productos.forEach(p => {
            doc.text(`- ${p.nombre}: ${p.cantidad} unidades`, 25, y);
            y += 7;
        });

        doc.save(`Resumen_Compra_${compra.fecha.replaceAll(" ", "_").replaceAll(":", "-")}.pdf`);
    };


    return (
        <div className="productos-container">
            <h1>Historial de Compras</h1>
            {historial.length === 0 ? (
                <p style={{ textAlign: "center", marginTop: "2rem", color: "#888" }}>
                    No has hecho ninguna compra todavía. Cuando la hagas aparecerá aquí el resumen de tu compra.
                </p>
            ) : (
                <div className="productos-list">
                    {historial.map((compra, index) => (
                        <div key={index} className="producto-card">
                            <h3>Compra #{index + 1}</h3>
                            <p><strong>Fecha:</strong> {compra.fecha}</p>
                            <p><strong>Método de Pago:</strong> {compra.metodoPago}</p>
                            <button onClick={() => setCompraSeleccionada(compra)}>Revisar Compra</button>
                        </div>
                    ))}
                </div>
            )}

            {compraSeleccionada && (
                <div className="modal-overlay" onClick={() => setCompraSeleccionada(null)}>
                    <div className="modal-contenido" onClick={(e) => e.stopPropagation()}>
                        <h2>Resumen de la Compra</h2>
                        {compraSeleccionada.productos.map((p, idx) => (
                            <p key={idx}><strong>{p.nombre}</strong> - {p.cantidad} unidades</p>
                        ))}
                        <p><strong>Total en Dinero:</strong> ${parseFloat(compraSeleccionada.totalDinero).toFixed(2)}</p>
                        <p><strong>Total en EcuniPoints:</strong> {compraSeleccionada.totalPuntos}</p>
                        <p><strong>Método de Pago:</strong> {compraSeleccionada.metodoPago}</p>
                        {compraSeleccionada.metodoPago === "dinero" ? (
                            <p>Tu compra fue hecha con dinero, así que recibirás {compraSeleccionada.ecuniPointsGanados} EcuniPoints.</p>
                        ) : (
                            <p>Tu compra fue realizada con EcuniPoints, por lo que no recibirás recompensa en esta compra.</p>
                        )}
                        <div className="modal-botones">
                            <button onClick={generarPDF}>Descargar Informe</button>
                            <button onClick={() => setCompraSeleccionada(null)}>Aceptar</button>
                        </div>
                    </div>
                </div>
            )}

            <div style={{ marginTop: "2rem", textAlign: "center" }}>
                <button onClick={() => navigate("/gestion-usuario")}>Volver a Gestión Usuario</button>
            </div>
        </div>
    );
}

export default HistorialCompras;