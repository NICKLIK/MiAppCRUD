import { useNavigate } from "react-router-dom";
import "./Productos.css";

function GestionCatalogo() {
    const navigate = useNavigate();

    return (
        <div className="productos-container">
            <h1>Gestion del Catalogo</h1>
            <div className="productos-list">
                <div className="producto-card" onClick={() => navigate("/categorias")}>
                    <h3>Gestionar Categorias</h3>
                    <p>Crear, editar o eliminar las categorias de productos.</p>
                </div>
                <div className="producto-card" onClick={() => navigate("/productos-admin")}>
                    <h3>Gestionar Productos</h3>
                    <p>Agregar, modificar o eliminar productos del catalogo.</p>
                </div>
                <div className="producto-card" onClick={() => navigate("/reabastecer-stock")}>
                    <h3>Reabastecer Stock</h3>
                    <p>Registrar solicitudes para productos agotados o por agotarse.</p>
                </div>
            </div>
        </div>
    );
}

export default GestionCatalogo;
