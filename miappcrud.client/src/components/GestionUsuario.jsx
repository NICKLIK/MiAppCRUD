import { useNavigate } from "react-router-dom";
import "./GestionAdmin.css";

function GestionUsuario() {
    const navigate = useNavigate();
    const correoUsuario = localStorage.getItem("usuario");

    const irACatalogo = () => navigate("/catalogo");
    const irAConfiguracionPerfil = () => navigate("/perfil");
    const irAListaDeseos = () => navigate("/lista-deseos");
    const irAHistorial = () => navigate("/historial-compras");

    return (
        <div className="gestion-admin-container">
            <h1>Bienvenido</h1>
            <p>Sesión iniciada como: <strong>{correoUsuario}</strong></p>

            <div className="gestion-admin-botones">
                <button onClick={irACatalogo}>Ver Catálogo de Productos</button>
                <button className="btn btn-info" onClick={irAListaDeseos}>Lista de Deseos</button>
                <button onClick={irAHistorial}>Ver Historial de Compras</button>
                <button onClick={irAConfiguracionPerfil}>Configurar Perfil</button>
            </div>
        </div>
    );
}

export default GestionUsuario;
