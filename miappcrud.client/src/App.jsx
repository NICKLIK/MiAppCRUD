import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './components/Login';
import Usuarios from './components/Usuarios';
import NoAutorizado from './components/NoAutorizado';
import { useState, useEffect } from 'react';
import './App.css';

function App() {
    const [estaLogueado, setEstaLogueado] = useState(false);

    useEffect(() => {
        const usuario = localStorage.getItem("usuario");
        setEstaLogueado(!!usuario);
    }, []);

    return (
        <Router>
            <Routes>
                <Route path="/" element={<Login setEstaLogueado={setEstaLogueado} />} />
                <Route path="/usuarios" element={estaLogueado ? <Usuarios /> : <Navigate to="/no-autorizado" />} />
                <Route path="/no-autorizado" element={<NoAutorizado />} />
            </Routes>
        </Router>
    );
}

export default App;
