import { useEffect, useState } from "react";
import "./Productos.css";

function GestionEventos() {
    const [nombre, setNombre] = useState("");
    const [fechaInicio, setFechaInicio] = useState("");
    const [fechaFin, setFechaFin] = useState("");
    const [descuento, setDescuento] = useState("");
    const [categorias, setCategorias] = useState([]);
    const [productos, setProductos] = useState([]);
    const [eventos, setEventos] = useState([]);

    const [modo, setModo] = useState("ninguno"); // ninguno | categorias | productos
    const [seleccionCategorias, setSeleccionCategorias] = useState([]);
    const [seleccionProductos, setSeleccionProductos] = useState([]);

    const cargarDatos = async () => {
        const cat = await fetch("https://localhost:52291/api/evento/categorias-stock-50").then(r => r.json());
        const prod = await fetch("https://localhost:52291/api/evento/productos-stock-50").then(r => r.json());
        const evts = await fetch("https://localhost:52291/api/evento").then(r => r.json());
        setCategorias(cat);
        setProductos(prod);
        setEventos(evts);
    };

    useEffect(() => {
        const cargarDatos = async () => {
            const cat = await fetch("https://localhost:52291/api/evento/categorias-stock-50").then(r => r.json());
            const prod = await fetch("https://localhost:52291/api/evento/productos-stock-50").then(r => r.json());
            const evts = await fetch("https://localhost:52291/api/evento").then(r => r.json());

            console.log("Categorias:", cat);
            console.log("Productos:", prod);

            setCategorias(cat);
            setProductos(prod);
            setEventos(evts);
        };

        cargarDatos();
    }, []);


    const crearEvento = async () => {
        if (!nombre || !fechaInicio || !fechaFin) {
            alert("Todos los campos son obligatorios excepto el descuento");
            return;
        }

        const dto = {
            nombre,
            fechaInicio,
            fechaFin,
            descuentoPorcentaje: descuento ? parseFloat(descuento) : null,
            idsProducto: []
        };

        let valido = true;

        if (modo === "categorias") {
            const res = await fetch("https://localhost:52291/api/evento/validar-categorias", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(seleccionCategorias)
            });
            valido = await res.json();

            if (valido) {
                const productosFiltrados = productos.filter(p =>
                    seleccionCategorias.includes(p.categoriaProductoId)
                );
                dto.idsProducto = productosFiltrados.map(p => p.id);
            }
        } else if (modo === "productos") {
            const res = await fetch("https://localhost:52291/api/evento/validar-productos", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(seleccionProductos)
            });
            valido = await res.json();
            dto.idsProducto = seleccionProductos;
        }

        if (!valido || dto.idsProducto.length === 0) {
            alert("Uno o más productos no tienen suficiente stock o no seleccionaste ninguno");
            return;
        }

        const res = await fetch("https://localhost:52291/api/evento", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        if (res.ok) {
            alert("Evento creado exitosamente");
            setNombre(""); setFechaInicio(""); setFechaFin(""); setDescuento("");
            setSeleccionCategorias([]); setSeleccionProductos([]); setModo("ninguno");
            cargarDatos();
        } else {
            alert("Error al crear el evento");
        }
    };


    return (
        <div className="productos-container">
            <h1>Gestionar Eventos</h1>
            <div>
                <input type="text" placeholder="Nombre del evento" value={nombre} onChange={(e) => setNombre(e.target.value)} />
                <input type="date" value={fechaInicio} onChange={(e) => setFechaInicio(e.target.value)} />
                <input type="date" value={fechaFin} onChange={(e) => setFechaFin(e.target.value)} />

                <div>
                    <button onClick={() => setModo("categorias")}>Elegir por Categorías</button>
                    <button onClick={() => setModo("productos")}>Elegir por Productos</button>
                </div>

                {modo === "categorias" && (
                    <div>
                        <label>Seleccione Categorías:</label>
                       // GestionEventos.jsx (solo el fragmento relevante actualizado)

                        <select
                            multiple
                            size={5}
                            style={{ width: "300px", margin: "10px auto", display: "block" }}
                            value={modo === "categorias" ? seleccionCategorias : seleccionProductos}
                            onChange={(e) => {
                                const selected = Array.from(e.target.selectedOptions).map(opt => parseInt(opt.value));
                                if (modo === "categorias") {
                                    setSeleccionCategorias(selected);
                                } else {
                                    setSeleccionProductos(selected);
                                }
                            }}
                        >
                            {(modo === "categorias" ? categorias : productos).map((item) => (
                                <option key={item.id} value={item.id}>
                                    {item.nombre}
                                </option>
                            ))}
                        </select>
                    </div>
                )}

                {modo === "productos" && (
                    <div>
                        <label>Seleccione Productos:</label>
                        <select
                            multiple
                            value={seleccionProductos}
                            onChange={(e) => {
                                const options = Array.from(e.target.selectedOptions).map(opt => Number(opt.value));
                                setSeleccionProductos(options);
                            }}
                            style={{ width: "250px", height: "150px" }}
                        >
                            {productos.map((p) => (
                                <option key={p.nombre + p.categoriaProductoId} value={p.id || p.nombre}>
                                    {p.nombre}
                                </option>
                            ))}
                        </select>
                    </div>
                )}

                <input type="number" placeholder="Descuento % (opcional)" value={descuento} onChange={(e) => setDescuento(e.target.value)} />
                <button onClick={crearEvento}>Crear Evento</button>
            </div>

            <h2>Eventos Registrados</h2>
            <div className="productos-list">
                {eventos.map(ev => (
                    <div key={ev.id} className="producto-card">
                        <h3>{ev.nombre}</h3>
                        <p>Desde: {new Date(ev.fechaInicio).toLocaleDateString()}</p>
                        <p>Hasta: {new Date(ev.fechaFin).toLocaleDateString()}</p>
                        <p>Descuento: {ev.descuentoPorcentaje ? `${ev.descuentoPorcentaje}%` : "Sin descuento"}</p>
                        <button onClick={() => alert("Editar pendiente")}>Editar</button>
                        <button onClick={async () => {
                            if (confirm("¿Seguro que deseas eliminar este evento?")) {
                                await fetch(`https://localhost:52291/api/evento/${ev.id}`, { method: "DELETE" });
                                cargarDatos();
                            }
                        }}>Eliminar</button>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default GestionEventos;
