import { useEffect, useState } from "react";
import jsPDF from "jspdf";
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
    const [mostrarAgotados, setMostrarAgotados] = useState(false);

    useEffect(() => {
        cargarSolicitudes();
        cargarProductos();

        const intervalo = setInterval(() => {
            cargarProductos();
        }, 5000); // Actualiza cada 5 segundos

        return () => clearInterval(intervalo);
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

        try {
            const res = await fetch(url, {
                method: metodo,
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (!res.ok) {
                const errorText = await res.text();
                alert("Error al crear solicitud: " + errorText);
                return;
            }

            cargarSolicitudes();
            setFormulario({ id: null, productoId: "", cantidad: "", fechaEntrega: "" });
            setModoEdicion(false);
        } catch (error) {
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

    const productosAgotados = productos.filter(p => p.stock <= 50);

    const generarPDFProductosAgotados = () => {
        const doc = new jsPDF();
        doc.setFontSize(14);
        doc.text("Informe de Productos por Agotarse", 20, 20);
        let y = 30;

        productosAgotados.forEach((p, index) => {
            doc.setFontSize(12);
            doc.text(`${index + 1}. ${p.nombre}`, 20, y); y += 7;
            doc.text(`   Descripción: ${p.descripcion}`, 20, y); y += 7;
            doc.text(`   Precio: $${p.precio}`, 20, y); y += 7;
            doc.text(`   Stock: ${p.stock}`, 20, y); y += 7;
            doc.text(`   EcuniPoints: ${p.ecuniPoints}`, 20, y); y += 7;
            doc.text(`   Categoría: ${p.categoriaNombre}`, 20, y); y += 10;

            if (y > 270) {
                doc.addPage();
                y = 20;
            }
        });

        doc.save("Productos_Agotados.pdf");
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
                            setFormulario({ id: null, productoId: "", cantidad: "", fechaEntrega: "" });
                        }}
                    >
                        Cancelar
                    </button>
                )}
            </form>

            <button onClick={() => setMostrarAgotados(true)} style={{ marginTop: "1rem" }}>
                Consultar Productos por Agotarse
            </button>

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

            {mostrarAgotados && (
                <div className="modal-overlay" onClick={() => setMostrarAgotados(false)}>
                    <div className="modal-contenido" onClick={(e) => e.stopPropagation()} style={{ maxHeight: "80vh", overflowY: "auto" }}>
                        <h2 style={{ color: "green", textAlign: "center" }}>Productos por Agotarse (Stock Menor o igual a 50)</h2>
                        {productosAgotados.map((p, idx) => (
                            <div key={idx} className="producto-card">
                                <img
                                    src={p.imagenUrl || "https://dummyimage.com/150x150/cccccc/000000&text=Sin+Imagen"}
                                    alt={p.nombre}
                                    className="producto-img"
                                />
                                <h3>{p.nombre}</h3>
                                <p>{p.descripcion}</p>
                                <p><strong>Precio:</strong> ${p.precio}</p>
                                <p><strong>Stock:</strong> {p.stock}</p>
                                <p><strong>EcuniPoints:</strong> {p.ecuniPoints}</p>
                                <p><strong>Categoría:</strong> {p.categoriaNombre}</p>
                            </div>
                        ))}
                        <div style={{ display: "flex", justifyContent: "center", gap: "1rem", marginTop: "1rem" }}>
                            <button onClick={generarPDFProductosAgotados}>Descargar Informe</button>
                            <button onClick={() => setMostrarAgotados(false)}>Regresar</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

export default ReabastecerStock;
