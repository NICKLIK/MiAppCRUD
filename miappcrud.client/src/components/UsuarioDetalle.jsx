import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "./Usuarios.css";

function UsuarioDetalle() {
    const { id } = useParams();
    const [usuario, setUsuario] = useState(null);
    const [historialCompras, setHistorialCompras] = useState([]);
    const [movimientosFondos, setMovimientosFondos] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchUsuario = async () => {
            const response = await fetch(`https://localhost:52291/api/usuario/${id}`);
            const data = await response.json();
            setUsuario(data);

            const historial = JSON.parse(localStorage.getItem(`historialCompras_${data.correo}`)) || [];
            setHistorialCompras(historial);

            const movimientos = JSON.parse(localStorage.getItem(`movimientosFondos_${data.correo}`)) || [];
            setMovimientosFondos(movimientos);
        };
        fetchUsuario();
    }, [id]);

    if (!usuario) return <p>Cargando...</p>;

    return (
        <div className="usuarios-container">
            <h2>Detalles del Usuario</h2>
            <div className="usuario-item">
                <h3>{usuario.nombre} {usuario.apellido}</h3>
                <p><strong>Correo:</strong> {usuario.correo}</p>
                <p><strong>Edad:</strong> {usuario.edad}</p>
                <p><strong>Género:</strong> {usuario.genero}</p>
                <p><strong>Ubicación:</strong> {usuario.ciudad}, {usuario.provincia}</p>
                <p><strong>Rol:</strong> {usuario.rol}</p>
            </div>

            <h3 style={{ marginTop: "2rem" }}>Historial de Compras Realizadas</h3>
            {historialCompras.length === 0 ? (
                <p>No hay compras registradas.</p>
            ) : (
                historialCompras.map((compra, index) => (
                    <div key={index} className="tarjeta-detalle">
                        <p><strong>Fecha:</strong> {compra.fecha}</p>
                        <p><strong>Método de pago:</strong> {compra.metodoPago}</p>
                        <p><strong>Total Dinero:</strong> ${compra.totalDinero}</p>
                        <p><strong>Total EcuniPoints:</strong> {compra.totalPuntos}</p>
                        {compra.metodoPago === "dinero" && (
                            <p><strong>EcuniPoints ganados:</strong> {compra.ecuniPointsGanados}</p>
                        )}
                        <p><strong>Productos:</strong></p>
                        <ul>
                            {compra.productos.map((p, i) => (
                                <li key={i}>{p.nombre} - {p.cantidad} unidades</li>
                            ))}
                        </ul>
                    </div>
                ))
            )}

            <h3 style={{ marginTop: "2rem" }}>Movimientos de Fondos</h3>
            {movimientosFondos.length === 0 ? (
                <p>No hay movimientos registrados.</p>
            ) : (
                movimientosFondos.map((mov, index) => (
                    <div key={index} className="tarjeta-detalle">
                        <p><strong>Fecha:</strong> {mov.fecha}</p>
                        <p><strong>Tipo:</strong> {mov.tipo}</p>
                        <p><strong>Cantidad:</strong> {mov.cantidad || mov.monto}</p>
                        <p><strong>Método:</strong> {mov.metodo || "Ingreso"}</p>
                    </div>
                ))
            )}

            <button onClick={() => navigate("/usuarios")} className="logout-button">
                Volver a la lista
            </button>
        </div>
    );
}
  
export default UsuarioDetalle;
