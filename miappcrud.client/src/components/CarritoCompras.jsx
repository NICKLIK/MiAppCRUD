import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import jsPDF from "jspdf";
import "./Productos.css";

function CarritoCompras() {
    const navigate = useNavigate();
    const correoUsuario = localStorage.getItem("usuario");

    const [carrito, setCarrito] = useState([]);
    const [productoAEliminar, setProductoAEliminar] = useState(null);
    const [cantidadAEliminar, setCantidadAEliminar] = useState(1);
    const [mostrarResumen, setMostrarResumen] = useState(false);
    const [mostrarPago, setMostrarPago] = useState(false);
    const [resumenCompra, setResumenCompra] = useState(null);
    const [fondos, setFondos] = useState({ dinero: 0, ecuniPoints: 0 });

    useEffect(() => {
        if (!correoUsuario) {
            navigate("/login");
            return;
        }
        const lista = localStorage.getItem(`carritoCompras_${correoUsuario}`);
        if (lista) {
            setCarrito(JSON.parse(lista));
        }
        const fondosGuardados = JSON.parse(localStorage.getItem(`fondos_${correoUsuario}`)) || { dinero: 0, ecuniPoints: 0 };
        setFondos(fondosGuardados);
    }, [correoUsuario]);

    const guardarCarrito = (nuevoCarrito) => {
        localStorage.setItem(`carritoCompras_${correoUsuario}`, JSON.stringify(nuevoCarrito));
        setCarrito(nuevoCarrito);
    };

    const registrarMovimiento = (tipo, cantidad, metodo) => {
        const movimientos = JSON.parse(localStorage.getItem(`movimientosFondos_${correoUsuario}`)) || [];
        movimientos.push({
            fecha: new Date().toLocaleString(),
            tipo,
            cantidad,
            metodo
        });
        localStorage.setItem(`movimientosFondos_${correoUsuario}`, JSON.stringify(movimientos));
    };

    const eliminarProducto = (id, cantidad, eliminarTodo = false) => {
        const actualizado = carrito.map(p => {
            if (p.id === id) {
                const nuevaCantidad = eliminarTodo ? 0 : p.cantidad - cantidad;
                return nuevaCantidad > 0 ? { ...p, cantidad: nuevaCantidad } : null;
            }
            return p;
        }).filter(p => p !== null);

        guardarCarrito(actualizado);
        setProductoAEliminar(null);
    };

    const totalPrecio = carrito.reduce((acc, p) => acc + p.precio * p.cantidad, 0);
    const totalPuntos = carrito.reduce((acc, p) => acc + p.ecuniPoints * p.cantidad, 0);

    const procesarPago = (metodo) => {
        if (carrito.length === 0) {
            alert("El carrito está vacío.");
            return;
        }

        let nuevosFondos = { ...fondos };
        let puntosGanados = 0;

        if (metodo === "dinero") {
            if (fondos.dinero < totalPrecio) {
                alert("Fondos insuficientes para pagar con dinero.");
                return;
            }
            nuevosFondos.dinero -= totalPrecio;
            puntosGanados = Math.floor(totalPrecio * 0.25);
            nuevosFondos.ecuniPoints += puntosGanados;
            registrarMovimiento("EGRESO", totalPrecio, "dinero");
            registrarMovimiento("INGRESO", puntosGanados, "ecuniPoints");
        } else if (metodo === "puntos") {
            if (fondos.ecuniPoints < totalPuntos) {
                alert("No tienes suficientes EcuniPoints.");
                return;
            }
            nuevosFondos.ecuniPoints -= totalPuntos;
            registrarMovimiento("EGRESO", totalPuntos, "ecuniPoints");
        }

        localStorage.setItem(`fondos_${correoUsuario}`, JSON.stringify(nuevosFondos));
        setFondos(nuevosFondos);
        localStorage.removeItem(`carritoCompras_${correoUsuario}`);
        setCarrito([]);

        const compra = {
            fecha: new Date().toLocaleString(),
            productos: carrito,
            totalDinero: totalPrecio.toFixed(2),
            totalPuntos,
            metodoPago: metodo,
            ecuniPointsGanados: puntosGanados
        };

        const historial = JSON.parse(localStorage.getItem(`historialCompras_${correoUsuario}`)) || [];
        historial.push(compra);
        localStorage.setItem(`historialCompras_${correoUsuario}`, JSON.stringify(historial));

        setResumenCompra(compra);
        setMostrarPago(false);
    };

    const generarPDF = () => {
        if (!resumenCompra) return;

        const doc = new jsPDF();
        doc.setFontSize(14);
        doc.text("Resumen de la Compra", 20, 20);

        let y = 30;

        doc.text(`Fecha: ${resumenCompra.fecha}`, 20, y); y += 10;
        doc.text(`Método de Pago: ${resumenCompra.metodoPago}`, 20, y); y += 10;
        doc.text(`Total en Dinero: $${resumenCompra.totalDinero}`, 20, y); y += 10;
        doc.text(`Total en EcuniPoints: ${resumenCompra.totalPuntos}`, 20, y); y += 10;

        if (resumenCompra.metodoPago === "dinero") {
            doc.text(`EcuniPoints ganados: ${resumenCompra.ecuniPointsGanados}`, 20, y);
            y += 10;
        }

        doc.text("Productos:", 20, y);
        y += 10;

        resumenCompra.productos.forEach(p => {
            doc.text(`- ${p.nombre}: ${p.cantidad} unidades`, 25, y);
            y += 8;
        });

        const fechaFormateada = resumenCompra.fecha
            .replace(",", "")
            .replace(":", "_")
            .replace(/ /g, "_")
            .replace(".", "");

        doc.save(`Resumen_Compra_${fechaFormateada}.pdf`);
    };

    return (
        <div className="productos-container">
            <h1>Carrito de Compras</h1>

            {carrito.length === 0 ? (
                <p style={{ textAlign: "center", marginTop: "2rem", color: "#888" }}>
                    No hay productos en el carrito.
                </p>
            ) : (
                <div className="productos-list">
                    {carrito.map((p) => (
                        <div key={p.id} className="producto-card">
                            <img src={p.imagenUrl} alt={p.nombre} className="producto-img" />
                            <h3 className="producto-name">{p.nombre}</h3>
                            <p>${p.precio} x {p.cantidad}</p>
                            <p>{p.ecuniPoints} EcuniPoints x {p.cantidad}</p>
                            <p><strong>Subtotal en Precio:</strong> ${(p.precio * p.cantidad).toFixed(2)}</p>
                            <p><strong>Subtotal en EcuniPoints:</strong> {p.ecuniPoints * p.cantidad}</p>
                            <p><strong>Stock restante:</strong> {p.stock - p.cantidad}</p>
                            <div className="modal-botones">
                                <button onClick={() => alert(`\n${p.nombre}\n\nDescripción: ${p.descripcion}\nPrecio: $${p.precio}\nEcuniPoints: ${p.ecuniPoints}\nCategoría: ${p.categoriaNombre}\nStock disponible: ${p.stock}`)}>
                                    Ver Producto
                                </button>
                                <button onClick={() => {
                                    setProductoAEliminar(p);
                                    setCantidadAEliminar(1);
                                }}>
                                    Eliminar
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {productoAEliminar && (
                <div className="modal-overlay" onClick={() => setProductoAEliminar(null)}>
                    <div className="modal-contenido" onClick={(e) => e.stopPropagation()}>
                        <h2 style={{ color: "green" }}>Eliminar producto</h2>
                        <p>{productoAEliminar.nombre}</p>
                        <p style={{ color: "black" }}>Seleccione la cantidad a eliminar:</p>
                        <input
                            type="number"
                            min="1"
                            max={productoAEliminar.cantidad}
                            value={cantidadAEliminar}
                            onChange={(e) => setCantidadAEliminar(parseInt(e.target.value))}
                        />
                        <div className="modal-botones">
                            <button onClick={() => eliminarProducto(productoAEliminar.id, cantidadAEliminar)}>Eliminar cantidad</button>
                            <button onClick={() => eliminarProducto(productoAEliminar.id, 0, true)}>Eliminar todo</button>
                            <button onClick={() => setProductoAEliminar(null)}>Cancelar</button>
                        </div>
                    </div>
                </div>
            )}

            {carrito.length > 0 && !mostrarResumen && (
                <div style={{ textAlign: "center", marginTop: "2rem" }}>
                    <button onClick={() => setMostrarResumen(true)}>Realizar Compra</button>
                    <button onClick={() => navigate("/catalogo")} style={{ marginLeft: "1rem" }}>Volver</button>
                </div>
            )}

            {mostrarResumen && (
                <div className="modal-overlay" onClick={() => setMostrarResumen(false)}>
                    <div className="modal-contenido" onClick={(e) => e.stopPropagation()}>
                        <h2 style={{ color: "green" }}>Resumen de Compra</h2>
                        {carrito.map(p => (
                            <div key={p.id}>
                                <p><strong>{p.nombre}</strong> - {p.cantidad} unidades</p>
                            </div>
                        ))}
                        <p><strong>Total precio:</strong> ${totalPrecio.toFixed(2)}</p>
                        <p><strong>Total EcuniPoints:</strong> {totalPuntos}</p>
                        <p>Este es el valor total de todos los productos seleccionados. Se puede elegir entre pagar con dinero o con EcuniPoints de ser el caso.</p>
                        <div className="modal-botones">
                            <button onClick={() => setMostrarPago(true)}>Elegir método de pago</button>
                            <button onClick={() => setMostrarResumen(false)}>Cancelar</button>
                        </div>
                    </div>
                </div>
            )}


            {mostrarPago && (
                <div className="modal-overlay" onClick={() => setMostrarPago(false)}>
                    <div className="modal-contenido" onClick={(e) => e.stopPropagation()}>
                        <h2 style={{ color: "green" }}>Elegir Método de Pago</h2>
                        <p>Total en Dinero: ${totalPrecio.toFixed(2)}</p>
                        <p>Total en EcuniPoints: {totalPuntos}</p>
                        <p>Con esta compra hecha por dinero ganarás: {Math.floor(totalPrecio * 0.25)} EcuniPoints</p>
                        <div className="modal-botones">
                            <button onClick={() => procesarPago("dinero")}>Pagar con Dinero</button>
                            <button onClick={() => procesarPago("puntos")}>Pagar con EcuniPoints</button>
                            <button onClick={() => setMostrarPago(false)}>Cancelar</button>
                        </div>
                    </div>
                </div>
            )}

            {resumenCompra && (
                <div className="modal-overlay" onClick={() => {
                    setResumenCompra(null);
                    navigate("/catalogo");
                }}>
                    <div className="modal-contenido" onClick={(e) => e.stopPropagation()}>
                        <h2 style={{ color: "green" }}>Compra Realizada</h2>
                        <p><strong>Fecha:</strong> {resumenCompra.fecha}</p>
                        {resumenCompra.productos.map((p, i) => (
                            <p key={i}>{p.nombre} - {p.cantidad} unidades</p>
                        ))}
                        <p><strong>Total Dinero:</strong> ${resumenCompra.totalDinero}</p>
                        <p><strong>Total EcuniPoints:</strong> {resumenCompra.totalPuntos}</p>
                        <p><strong>Método de Pago:</strong> {resumenCompra.metodoPago === "dinero" ? "Dinero" : "EcuniPoints"}</p>
                        {resumenCompra.metodoPago === "dinero" ? (
                            <p>Tu compra fue hecha con dinero, así que recibirás <strong>{resumenCompra.ecuniPointsGanados} EcuniPoints</strong>.</p>
                        ) : (
                            <p>Tu compra fue realizada con EcuniPoints, por lo que <strong>no recibirás recompensa</strong>.</p>
                        )}
                        <div className="modal-botones">
                            <button onClick={generarPDF}>Descargar Informe de Pago</button>
                            <button onClick={() => {
                                setResumenCompra(null);
                                navigate("/catalogo");
                            }}>Aceptar</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

export default CarritoCompras;
