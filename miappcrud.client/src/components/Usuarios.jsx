import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "./Usuarios.css"; 

function Usuarios() {
    const [usuarios, setUsuarios] = useState([]);
    const navigate = useNavigate();

    const cargarUsuarios = async () => {
        const response = await fetch("https://localhost:52291/api/usuario");
        const data = await response.json();
        setUsuarios(data);
    };

    useEffect(() => {
        cargarUsuarios();
    }, []);

    const cerrarSesion = () => {
        localStorage.removeItem("usuario");
        navigate("/");
    };

    const eliminar = async (id) => {
        await fetch(`https://localhost:52291/api/usuario/${id}`, { method: "DELETE" });
        cargarUsuarios();
    };

    return (
        <div className="usuarios-container">
            <h1>Informacion de los Usuarios Registrados</h1>
            <ul>
                {usuarios.map((u) => (
                    <li key={u.id} className="usuario-item">
                        {u.nombreUsuario}
                        <button onClick={() => eliminar(u.id)}>Eliminar</button>
                    </li>
                ))}
            </ul>
            <button className="logout-button" onClick={cerrarSesion}>Cerrar sesion</button>
        </div>
    );
}

export default Usuarios;
