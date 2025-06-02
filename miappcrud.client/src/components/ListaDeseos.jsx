import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "./Productos.css";

function ListaDeseos() {
    const navigate = useNavigate();
    const correoUsuario = localStorage.getItem("usuario");
    const [productos, setProductos] = useState([]);
    const [stockGlobal, setStockGlobal] = useState({}); // Para mostrar el stock actualizado real

    useEffect(() => {
        if (!correoUsuario) {
            navigate("/login");
            return;
        }

        const lista = JSON.parse(localStorage.getItem(`listaDeseos_${correoUsuario}`)) || [];
        setProductos(lista);
        cargarStock(lista);
    }, [correoUsuario]);

    const cargarStock = async (productosDeseados) => {
        try {
            const response = await fetch("https://localhost:52291/api/producto");
            const productosBD = await response.json();

            const carrito = JSON.parse(localStorage.getItem(`carritoCompras_${correoUsuario}`)) || [];

            const stockReal = {};
            productosBD.forEach(prod => {
                const enCarrito = carrito.find(p => p.id === prod.id);
                const enDeseos = productosDeseados.find(p => p.id === prod.id);
                const cantidadOcupada = (enCarrito?.cantidad || 0) + (enDeseos?.cantidad || 0);
                stockReal[prod.id] = prod.stock - cantidadOcupada + (enDeseos?.cantidad || 0);
            });

            setStockGlobal(stockReal);
        } catch (error) {
            console.error("Error al cargar stock actualizado:", error);
        }
    };

    const eliminarProducto = (id) => {
        const nuevos = productos.filter(p => p.id !== id);
        setProductos(nuevos);
        localStorage.setItem(`listaDeseos_${correoUsuario}`, JSON.stringify(nuevos));
    };

    const comprarProducto = (producto) => {
        const keyCarrito = `carritoCompras_${correoUsuario}`;
        const carrito = JSON.parse(localStorage.getItem(keyCarrito)) || [];

        const existente = carrito.find(p => p.id === producto.id);
        if (existente) {
            existente.cantidad += producto.cantidad;
        } else {
            carrito.push({ ...producto });
        }

        localStorage.setItem(keyCarrito, JSON.stringify(carrito));

        // Eliminar de lista de deseos
        eliminarProducto(producto.id);
        alert("Producto añadido al carrito.");
    };

    return (
        <div className="productos-container">
            <h1>Lista de Deseos</h1>
            {productos.length === 0 ? (
                <p style={{ textAlign: "center", marginTop: "2rem", color: "#888" }}>
                    No hay productos en la lista de deseos.
                </p>
            ) : (
                <div className="productos-list">
                    {productos.map((p) => (
                        <div key={p.id} className="producto-card">
                            <img src={p.imagenUrl} alt={p.nombre} className="producto-img" />
                            <h3 className="producto-name">{p.nombre}</h3>
                            <p className="producto-price">${p.precio}</p>
                            <p>{p.ecuniPoints} EcuniPoints</p>
                            <p><strong>Categoría:</strong> {p.categoriaNombre || "Sin categoría"}</p>
                            <p><strong>Cantidad reservada:</strong> {p.cantidad}</p>
                            <p><strong>Stock actualizado:</strong> {stockGlobal[p.id] ?? p.stock}</p>

                            <div className="modal-botones">
                                <button onClick={() => comprarProducto(p)}>Comprar producto</button>
                                <button onClick={() => eliminarProducto(p.id)}>Eliminar</button>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            <div style={{ marginTop: "2rem", textAlign: "center" }}>
                <button onClick={() => navigate("/gestion-usuario")}>Volver a Gestión Usuario</button>
            </div>
        </div>
    );
}

export default ListaDeseos;
