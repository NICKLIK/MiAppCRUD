import { useEffect, useState } from "react";
import "./Productos.css";

function ReabastecerStock() {
    const [solicitudes, setSolicitudes] = useState([]);
    const [productos, setProductos] = useState([]);
    const [formulario, setFormulario] = useState({
        id: null,
        productoId: "",
        cantidad: "",
        fechaEntrega: ""
    });
    const [modoEdicion, setModoEdicion] = useState(false);

    useEffect(() => {
        cargarSolicitudes();
        cargarProductos();
    }, []);

    const cargarSolicitudes = async () => {
        const res = await fetch("https://localhost:52291/api/ReabastecimientoStock");
        const data = await res.json();
        setSolicitudes(data);
    };

    const cargarProductos = async () => {
        const res = await fetch("https://localhost:52291/api/producto");
        const data = await res.json();
        setProductos(data);
    };

    const manejarCambio = (e) => {
        const { name, value } = e.target;
        setFormulario(prev => ({ ...prev, [name]: value }));
    };

    const manejarEnvio = async (e) => {
        e.preventDefault();

        const url = modoEdicion
            ? `https://localhost:52291/api/ReabastecimientoStock/${formulario.id}`
            : "https://localhost:52291/api/ReabastecimientoStock";

        const metodo = modoEdicion ? "PUT" : "POST";

        const payload = {
            productoId: parseInt(formulario.productoId),
            cantidad: parseInt(formulario.cantidad),
            fechaEntrega: new Date(formulario.fechaEntrega).toISOString()

        };

        if (modoEdicion) payload.id = formulario.id;

        console.log("Payload a enviar:", payload);

        try {
            const res = await fetch(url, {
                method: metodo,
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (!res.ok) {
                const errorText = await res.text();
                console.error("Error al enviar:", errorText);
                alert("Error al crear solicitud: " + errorText);
                return;
            }

            cargarSolicitudes();
            setFormulario({
                id: null,
                productoId: "",
                cantidad: "",
                fechaEntrega: ""
            });
            setModoEdicion(false);
        } catch (error) {
            console.error("Error de red:", error);
            alert("Error de red: " + error.message);
        }
    };

    const manejarEditar = (sol) => {
        setFormulario({
            id: sol.id,
            productoId: sol.productoId,
            cantidad: sol.cantidad,
            fechaEntrega: sol.fechaEntrega?.split("T")[0] || ""
        });
        setModoEdicion(true);
    };

    const manejarEliminar = async (id) => {
        if (window.confirm("¿Deseas eliminar esta solicitud?")) {
            await fetch(`https://localhost:52291/api/ReabastecimientoStock/${id}`, {
                method: "DELETE"
            });
            cargarSolicitudes();
        }
    };

    return (
        <div className="productos-container">
            <h1>Reabastecimiento de Stock</h1>

            <form className="producto-form" onSubmit={manejarEnvio}>
                <select name="productoId" value={formulario.productoId} onChange={manejarCambio} required>
                    <option value="">Seleccione un producto</option>
                    {productos.map(p => (
                        <option key={p.id} value={p.id}>{p.nombre}</option>
                    ))}
                </select>

                <input
                    name="cantidad"
                    type="number"
                    placeholder="Cantidad"
                    value={formulario.cantidad}
                    onChange={manejarCambio}
                    required
                />

                <input
                    name="fechaEntrega"
                    type="date"
                    placeholder="Fecha de entrega"
                    value={formulario.fechaEntrega}
                    onChange={manejarCambio}
                    required
                />

                <button type="submit">{modoEdicion ? "Actualizar" : "Crear"} Solicitud</button>
                {modoEdicion && (
                    <button
                        type="button"
                        onClick={() => {
                            setModoEdicion(false);
                            setFormulario({
                                id: null,
                                productoId: "",
                                cantidad: "",
                                fechaEntrega: ""
                            });
                        }}
                    >
                        Cancelar
                    </button>
                )}
            </form>

            <div className="productos-list">
                {solicitudes.map((s) => (
                    <div key={s.id} className="producto-card">
                        <h3>{s.producto?.nombre}</h3>
                        <p><strong>Cantidad solicitada:</strong> {s.cantidad}</p>
                        <p><strong>Estado:</strong> {s.estado}</p>
                        <p><strong>Fecha entrega:</strong> {new Date(s.fechaEntrega).toLocaleDateString()}</p>
                        <div className="producto-actions">
                            <button onClick={() => manejarEditar(s)}>Editar</button>
                            <button onClick={() => manejarEliminar(s.id)}>Eliminar</button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default ReabastecerStock;
