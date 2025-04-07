import { useNavigate } from "react-router-dom";

function NoAutorizado() {
    const navigate = useNavigate();

    return (
        <div>
            <h1>No estas autorizado, Debes Iniciar Sesion</h1>
            <button onClick={() => navigate("/")}>Iniciar sesion</button>
        </div>
    );
}

export default NoAutorizado;
