import { useState, useEffect } from "react";
import "./Productos.css";

function ProductosAdmin() {
    const [productos, setProductos] = useState([]);
    const [categorias, setCategorias] = useState([]);
    const [formulario, setFormulario] = useState({
        id: null,
        nombre: "",
        descripcion: "",
        precio: "",
        stock: "",
        imagenUrl: "",
        ecuniPoints: "",
        categoriaProductoId: ""
    });
    const [modoEdicion, setModoEdicion] = useState(false);

    useEffect(() => {
        cargarProductos();
        cargarCategorias();
    }, []);

    const cargarProductos = async () => {
        const res = await fetch("https://localhost:52291/api/producto");
        const data = await res.json();
        setProductos(data);
    };

    const cargarCategorias = async () => {
        const res = await fetch("https://localhost:52291/api/categoriaproducto");
        const data = await res.json();
        // Filtramos categorías inválidas (id 0 o null)
        const filtradas = data.filter(c => c.id && c.id !== 0);
        setCategorias(filtradas);
    };

    const manejarCambio = (e) => {
        const { name, value } = e.target;
        const valorConvertido =
            name === "categoriaProductoId" ||
                name === "stock" ||
                name === "precio" ||
                name === "ecuniPoints"
                ? Number(value)
                : value;

        setFormulario(prev => ({ ...prev, [name]: valorConvertido }));
    };

    const manejarEnvio = async (e) => {
        e.preventDefault();

        const url = modoEdicion
            ? `https://localhost:52291/api/producto/${formulario.id}`
            : "https://localhost:52291/api/producto";

        const metodo = modoEdicion ? "PUT" : "POST";

        const bodyData = modoEdicion
            ? { ...formulario } // incluye ID
            : {
                nombre: formulario.nombre,
                descripcion: formulario.descripcion,
                precio: formulario.precio,
                stock: formulario.stock,
                imagenUrl: formulario.imagenUrl,
                ecuniPoints: formulario.ecuniPoints,
                categoriaProductoId: formulario.categoriaProductoId
            };

        try {
            const res = await fetch(url, {
                method: metodo,
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyData)
            });

            if (!res.ok) {
                const contentType = res.headers.get("content-type");
                let errorMessage;

                if (contentType && contentType.includes("application/json")) {
                    const error = await res.json();
                    errorMessage = error.mensaje || "Error desconocido";
                } else {
                    const text = await res.text();
                    errorMessage = "Error inesperado: " + text;
                }

                alert(errorMessage);
                return;
            }

            cargarProductos();
            setFormulario({
                id: null,
                nombre: "",
                descripcion: "",
                precio: "",
                stock: "",
                imagenUrl: "",
                ecuniPoints: "",
                categoriaProductoId: ""
            });
            setModoEdicion(false);
        } catch (err) {
            alert("Error inesperado: " + err.message);
            console.error(err);
        }
    };


    const manejarEditar = (producto) => {
        setFormulario({
            id: producto.id,
            nombre: producto.nombre,
            descripcion: producto.descripcion,
            precio: producto.precio,
            stock: producto.stock,
            imagenUrl: producto.imagenUrl,
            ecuniPoints: producto.ecuniPoints,
            categoriaProductoId: producto.categoriaProductoId
        });
        setModoEdicion(true);
    };

    const manejarEliminar = async (id) => {
        if (confirm("¿Estás seguro de eliminar este producto?")) {
            await fetch(`https://localhost:52291/api/producto/${id}`, { method: "DELETE" });
            cargarProductos();
        }
    };

    return (
        <div className="productos-container">
            <h1>Gestión de Productos</h1>
            <form className="producto-form" onSubmit={manejarEnvio}>
                <input name="nombre" value={formulario.nombre} onChange={manejarCambio} placeholder="Nombre" required />
                <textarea name="descripcion" value={formulario.descripcion} onChange={manejarCambio} placeholder="Descripción" />
                <input name="precio" type="number" value={formulario.precio} onChange={manejarCambio} placeholder="Precio" required />
                <input name="stock" type="number" value={formulario.stock} onChange={manejarCambio} placeholder="Stock" required />
                <input name="imagenUrl" value={formulario.imagenUrl} onChange={manejarCambio} placeholder="URL de la imagen" />
                <input name="ecuniPoints" type="number" value={formulario.ecuniPoints} onChange={manejarCambio} placeholder="EcuniPoints" required />

                <select name="categoriaProductoId" value={formulario.categoriaProductoId} onChange={manejarCambio} required>
                    <option value="">Selecciona una categoría</option>
                    {categorias.map(c => (
                        <option key={c.id} value={c.id}>{c.nombre}</option>
                    ))}
                </select>

                <button type="submit">{modoEdicion ? "Actualizar" : "Crear"} Producto</button>
            </form>

            <div className="productos-list">
                {productos.map((p) => (
                    <div key={p.id} className="producto-card">
                        <img src={p.imagenUrl} alt={p.nombre} className="producto-img" />
                        <h3>{p.nombre}</h3>
                        <p>{p.descripcion}</p>
                        <p><strong>Precio:</strong> ${p.precio}</p>
                        <p><strong>Stock:</strong> {p.stock}</p>
                        <p><strong>EcuniPoints:</strong> {p.ecuniPoints}</p>
                        <p><strong>Categoría:</strong> {p.categoriaNombre}</p>
                        <div className="producto-actions">
                            <button onClick={() => manejarEditar(p)}>Editar</button>
                            <button onClick={() => manejarEliminar(p.id)}>Eliminar</button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default ProductosAdmin;
