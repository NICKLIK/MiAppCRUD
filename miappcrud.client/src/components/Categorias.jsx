import { useEffect, useState } from "react";
import "./Productos.css";

function Categorias() {
    const [categorias, setCategorias] = useState([]);
    const [formData, setFormData] = useState({ nombre: "", id: null });
    const [modoEdicion, setModoEdicion] = useState(false);

    const cargarCategorias = async () => {
        try {
            const response = await fetch("https://localhost:52291/api/categoriaproducto");
            const data = await response.json();
            
            const filtradas = data.filter(c => c.id && c.id !== 0);
            setCategorias(filtradas);
        } catch (error) {
            console.error("Error al cargar categor�as:", error);
        }
    };

    useEffect(() => {
        cargarCategorias();
    }, []);

    const handleChange = (e) => {
        setFormData({ ...formData, nombre: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const url = modoEdicion
            ? `https://localhost:52291/api/categoriaproducto/${formData.id}`
            : "https://localhost:52291/api/categoriaproducto";

        const method = modoEdicion ? "PUT" : "POST";

        const bodyData = { nombre: formData.nombre };
        if (modoEdicion) bodyData.id = formData.id;

        try {
            const response = await fetch(url, {
                method,
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyData)
            });

            if (!response.ok) {
                const errorText = await response.text();
                console.error("Error al guardar la categor�a:", errorText);
                alert("No se pudo guardar la categor�a");
                return;
            }

            setFormData({ nombre: "", id: null });
            setModoEdicion(false);
            cargarCategorias();
        } catch (error) {
            console.error("Error:", error);
            alert("Error inesperado");
        }
    };

    const editarCategoria = (categoria) => {
        setFormData({ nombre: categoria.nombre, id: categoria.id });
        setModoEdicion(true);
    };

    const eliminarCategoria = async (id) => {
        if (!window.confirm("�Est�s seguro de eliminar esta categor�a?")) return;
        if (!id || id === 0) {
            alert("Categor�a inv�lida para eliminar");
            return;
        }

        try {
            const res = await fetch(`https://localhost:52291/api/categoriaproducto/${id}`, {
                method: "DELETE"
            });

            if (!res.ok) {
                const error = await res.text();
                console.error("Error al eliminar categor�a:", error);
                alert("No se pudo eliminar la categor�a.");
                return;
            }

            cargarCategorias();
        } catch (error) {
            console.error("Error al eliminar categor�a:", error);
        }
    };

    return (
        <div className="productos-container">
            <h1>Gesti�n de Categor�as</h1>

            <form onSubmit={handleSubmit} className="producto-card">
                <input
                    type="text"
                    value={formData.nombre}
                    onChange={handleChange}
                    placeholder="Nombre de la categor�a"
                    required
                />
                <button type="submit">{modoEdicion ? "Actualizar" : "Agregar"}</button>
                {modoEdicion && (
                    <button type="button" onClick={() => {
                        setModoEdicion(false);
                        setFormData({ nombre: "", id: null });
                    }}>
                        Cancelar
                    </button>
                )}
            </form>

            <div className="productos-list">
                {categorias.map((cat) => (
                    <div key={cat.id} className="producto-card">
                        <h3>{cat.nombre}</h3>
                        <div>
                            <button onClick={() => editarCategoria(cat)}>Editar</button>
                            <button onClick={() => eliminarCategoria(cat.id)}>Eliminar</button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default Categorias;
