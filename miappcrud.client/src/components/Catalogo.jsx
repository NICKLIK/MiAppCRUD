import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "./Productos.css";

function Catalogo() {
    const [productos, setProductos] = useState([]);
    const [productoSeleccionado, setProductoSeleccionado] = useState(null);
    const [categorias, setCategorias] = useState([]);
    const [busquedaTemp, setBusquedaTemp] = useState("");
    const [categoriaTemp, setCategoriaTemp] = useState("");
    const [busqueda, setBusqueda] = useState("");
    const [categoriaSeleccionada, setCategoriaSeleccionada] = useState("");
    const [reservados, setReservados] = useState([]);
    const [cantidadCarrito, setCantidadCarrito] = useState(1);
    const [mostrarFormularioCarrito, setMostrarFormularioCarrito] = useState(false);
    const [mostrarIngresoFondos, setMostrarIngresoFondos] = useState(false);
    const [montoIngreso, setMontoIngreso] = useState("");
    const [fondos, setFondos] = useState({ dinero: 0, ecuniPoints: 0 });

    const correoUsuario = localStorage.getItem("usuario");
    const navigate = useNavigate();

    useEffect(() => {
        cargarProductos();
        cargarCategorias();
        cargarReservados();
        cargarFondos();
    }, []);

    const cargarFondos = () => {
        const data = JSON.parse(localStorage.getItem(`fondos_${correoUsuario}`)) || { dinero: 0, ecuniPoints: 0 };
        setFondos(data);
    };

    const actualizarFondos = (nuevosFondos) => {
        setFondos(nuevosFondos);
        localStorage.setItem(`fondos_${correoUsuario}`, JSON.stringify(nuevosFondos));
    };

    const registrarMovimientoFondos = (tipo, monto) => {
        const historialFondos = JSON.parse(localStorage.getItem(`movimientosFondos_${correoUsuario}`)) || [];
        const nuevoMovimiento = {
            fecha: new Date().toLocaleString(),
            tipo,
            monto
        };
        historialFondos.push(nuevoMovimiento);
        localStorage.setItem(`movimientosFondos_${correoUsuario}`, JSON.stringify(historialFondos));
    };

    const confirmarIngreso = () => {
        const monto = parseFloat(montoIngreso);
        if (isNaN(monto) || monto <= 0) {
            alert("Ingrese un monto válido");
            return;
        }

        const nuevos = { ...fondos, dinero: fondos.dinero + monto };
        actualizarFondos(nuevos);
        registrarMovimientoFondos("Ingreso", monto);
        setMostrarIngresoFondos(false);
        setMontoIngreso("");
        alert("Fondos ingresados con éxito");
    };

    const cargarProductos = async () => {
        try {
            const response = await fetch("https://localhost:52291/api/producto");
            const data = await response.json();

            const carrito = JSON.parse(localStorage.getItem(`carritoCompras_${correoUsuario}`)) || [];
            const listaDeseos = JSON.parse(localStorage.getItem(`listaDeseos_${correoUsuario}`)) || [];
           // const stockActualizado = JSON.parse(localStorage.getItem("stock_actualizado")) || {};

            const productosAjustados = data.map(prod => {
                const enCarrito = carrito.find(p => p.id === prod.id);
                const enDeseos = listaDeseos.find(p => p.id === prod.id);
                const cantidadOcupada = (enCarrito?.cantidad || 0) + (enDeseos?.cantidad || 0);
                const stockBase = prod.stock;
                return { ...prod, stock: stockBase - cantidadOcupada };
            });

            setProductos(productosAjustados);
        } catch (error) {
            console.error("Error al cargar productos:", error);
        }
    };

    const cargarCategorias = async () => {
        try {
            const response = await fetch("https://localhost:52291/api/categoriaproducto");
            const data = await response.json();
            setCategorias(data);
        } catch (error) {
            console.error("Error al cargar categorías:", error);
        }
    };

    const cargarReservados = () => {
        if (correoUsuario) {
            const key = `listaDeseos_${correoUsuario}`;
            const lista = JSON.parse(localStorage.getItem(key)) || [];
            setReservados(lista);
        }
    };

    const aplicarFiltros = () => {
        setBusqueda(busquedaTemp);
        setCategoriaSeleccionada(categoriaTemp);
    };

    const productosFiltrados = productos.filter((p) => {
        const coincideBusqueda = p.nombre.toLowerCase().includes(busqueda.toLowerCase());
        const coincideCategoria = categoriaSeleccionada === "" || p.categoriaProductoId === parseInt(categoriaSeleccionada);
        return coincideBusqueda && coincideCategoria;
    });

    const cerrarDetalle = () => {
        setProductoSeleccionado(null);
        setMostrarFormularioCarrito(false);
        setCantidadCarrito(1);
    };

    const estaReservado = (idProducto) => {
        return reservados.some(p => p.id === idProducto);
    };

    const confirmarCarrito = () => {
        if (cantidadCarrito <= 0) {
            alert("La cantidad debe ser mayor a 0");
            return;
        }

        if (cantidadCarrito > productoSeleccionado.stock) {
            alert("La cantidad seleccionada supera el stock disponible");
            return;
        }

        const key = `carritoCompras_${correoUsuario}`;
        const carrito = JSON.parse(localStorage.getItem(key)) || [];
        const existente = carrito.find(p => p.id === productoSeleccionado.id);

        if (existente) {
            existente.cantidad += cantidadCarrito;
        } else {
            carrito.push({ ...productoSeleccionado, cantidad: cantidadCarrito });
        }

        localStorage.setItem(key, JSON.stringify(carrito));
        alert("Producto agregado al carrito");

        setProductos(prev => prev.map(prod => {
            if (prod.id === productoSeleccionado.id) {
                return { ...prod, stock: prod.stock - cantidadCarrito };
            }
            return prod;
        }));

        cerrarDetalle();
    };

    const confirmarReserva = () => {
        if (cantidadCarrito <= 0) {
            alert("La cantidad debe ser mayor a 0");
            return;
        }

        if (cantidadCarrito > productoSeleccionado.stock) {
            alert("La cantidad supera el stock disponible");
            return;
        }

        const key = `listaDeseos_${correoUsuario}`;
        const lista = JSON.parse(localStorage.getItem(key)) || [];
        const existente = lista.find(p => p.id === productoSeleccionado.id);

        if (existente) {
            alert("Este producto ya está reservado");
            return;
        }

        const productoReservado = { ...productoSeleccionado, cantidad: cantidadCarrito };
        lista.push(productoReservado);
        localStorage.setItem(key, JSON.stringify(lista));

        setProductos(prev => prev.map(prod => {
            if (prod.id === productoSeleccionado.id) {
                return { ...prod, stock: prod.stock - cantidadCarrito };
            }
            return prod;
        }));

        alert("Producto reservado con éxito");
        cerrarDetalle();
    };


    return (
        <div className="productos-container">
            <h1>Catálogo de Productos</h1>

            <div style={{ textAlign: "center", marginBottom: "1rem" }}>
                <h3>Fondos Actuales</h3>
                <p>Dinero: ${fondos.dinero.toFixed(2)} &nbsp;&nbsp; | &nbsp;&nbsp; EcuniPoints: {fondos.ecuniPoints}</p>
                <button onClick={() => setMostrarIngresoFondos(true)}>Ingresar Fondos</button>
            </div>

            <div style={{ marginBottom: "1rem", display: "flex", gap: "1rem", alignItems: "center", justifyContent: "center" }}>
                <input
                    type="text"
                    placeholder="Buscar por nombre..."
                    value={busquedaTemp}
                    onChange={(e) => setBusquedaTemp(e.target.value)}
                />
                <select value={categoriaTemp} onChange={(e) => setCategoriaTemp(e.target.value)}>
                    <option value="">Todas las categorías</option>
                    {categorias.map((cat) => (
                        <option key={cat.id} value={cat.id}>{cat.nombre}</option>
                    ))}
                </select>
                <button onClick={aplicarFiltros}>Buscar</button>
                <button onClick={() => navigate("/carrito-compras")}>Ver Carrito de Compras</button>
            </div>

            {productosFiltrados.length > 0 ? (
                <div className="productos-list">
                    {productosFiltrados.map((p) => (
                        <div key={p.id} className="producto-card" onClick={() => setProductoSeleccionado(p)}>
                            <img src={p.imagenUrl} alt={p.nombre} className="producto-img" />
                            <h3 className="producto-name">{p.nombre}</h3>
                            <p className="producto-price">${p.precio}</p>
                            <p>{p.ecuniPoints} EcuniPoints</p>
                        </div>
                    ))}
                </div>
            ) : (
                <p style={{ textAlign: "center", marginTop: "2rem", color: "#888" }}>
                    No se ha encontrado ningún producto relacionado.
                </p>
            )}

            {productoSeleccionado && (
                <div className="modal-overlay" onClick={cerrarDetalle}>
                    <div className="modal-contenido" onClick={(e) => e.stopPropagation()}>
                        <img
                            src={productoSeleccionado.imagenUrl || "https://dummyimage.com/150x150/cccccc/000000&text=Sin+Imagen"}
                            alt={productoSeleccionado.nombre}
                            className="producto-img"
                        />
                        <h2>{productoSeleccionado.nombre}</h2>
                        <p><strong>Descripcion:</strong> {productoSeleccionado.descripcion}</p>
                        <p><strong>Precio:</strong> ${productoSeleccionado.precio}</p>
                        <p><strong>EcuniPoints:</strong> {productoSeleccionado.ecuniPoints}</p>
                        <p><strong>Categoria:</strong> {productoSeleccionado.categoriaNombre || "Sin categoría"}</p>
                        <p><strong>Stock disponible:</strong> {productoSeleccionado.stock}</p>

                        {!mostrarFormularioCarrito ? (
                            <div className="modal-botones">
                                <button onClick={cerrarDetalle}>Regresar</button>
                                <button onClick={() => setMostrarFormularioCarrito(true)}>Añadir al carrito</button>
                                <button
                                    onClick={() => setMostrarFormularioCarrito("reservar")}
                                    disabled={estaReservado(productoSeleccionado.id)}
                                    style={{
                                        backgroundColor: estaReservado(productoSeleccionado.id) ? "#ccc" : "",
                                        cursor: estaReservado(productoSeleccionado.id) ? "not-allowed" : "pointer"
                                    }}
                                >
                                    {estaReservado(productoSeleccionado.id) ? "Ya reservado" : "Reservar producto"}
                                </button>
                            </div>
                        ) : mostrarFormularioCarrito === true ? (
                            <div>
                                <label style={{ color: "black" }}>Seleccione cantidad:</label>
                                <input
                                    type="number"
                                    min="1"
                                    max={productoSeleccionado.stock}
                                    value={cantidadCarrito}
                                    onChange={(e) => setCantidadCarrito(parseInt(e.target.value))}
                                />
                                <div className="modal-botones">
                                    <button onClick={confirmarCarrito}>Confirmar Pedido</button>
                                    <button onClick={cerrarDetalle}>Cancelar</button>
                                </div>
                            </div>
                        ) : (
                            <div>
                                <label style={{ color: "black" }}>Seleccione cantidad a reservar:</label>
                                <input
                                    type="number"
                                    min="1"
                                    max={productoSeleccionado.stock}
                                    value={cantidadCarrito}
                                    onChange={(e) => setCantidadCarrito(parseInt(e.target.value))}
                                />
                                <div className="modal-botones">
                                    <button onClick={confirmarReserva}>Confirmar Reserva</button>
                                    <button onClick={cerrarDetalle}>Cancelar</button>
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            )}

            {mostrarIngresoFondos && (
                <div className="modal-overlay" onClick={() => setMostrarIngresoFondos(false)}>
                    <div className="modal-contenido" onClick={(e) => e.stopPropagation()}>
                        <h2 style={{ color: "green" }}>Ingresar Fondos</h2>
                        <label style={{ color: "black" }}>Monto a ingresar ($):</label>
                        <input
                            type="number"
                            min="0"
                            value={montoIngreso}
                            onChange={(e) => setMontoIngreso(e.target.value)}
                        />
                        <div className="modal-botones">
                            <button onClick={confirmarIngreso}>Confirmar Ingreso</button>
                            <button onClick={() => setMostrarIngresoFondos(false)}>Cancelar</button>
                        </div>
                    </div>
                </div>
            )}

        </div>
    );
}

export default Catalogo;
