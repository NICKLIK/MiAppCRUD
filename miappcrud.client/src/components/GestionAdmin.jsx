import { useNavigate } from "react-router-dom";
import "./GestionAdmin.css";

function GestionAdmin() {
    const navigate = useNavigate();

    return (
        <div className="gestion-admin-container">
            <h1>Panel de Administracion</h1>
            <div className="gestion-admin-botones">
                <button onClick={() => navigate("/usuarios")}>Gestionar Usuarios</button>
                <button onClick={() => navigate("/catalogo-admin")}>Gestionar Catalogo de Productos</button>
            </div>
        </div>
    );
}

export default GestionAdmin;
