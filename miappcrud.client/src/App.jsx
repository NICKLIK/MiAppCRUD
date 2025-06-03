import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './components/Login';
import LoginAdmin from './components/LoginAdmin'; 
import Usuarios from './components/Usuarios';
import NoAutorizado from './components/NoAutorizado';
import UsuarioDetalle from "./components/UsuarioDetalle";
import Catalogo from "./components/Catalogo";
import GestionCatalogo from "./components/GestionCatalogo";
import GestionAdmin from "./components/GestionAdmin"; 
import ProductosAdmin from './components/ProductosAdmin';
import Categorias from './components/Categorias';
import ReabastecerStock from "./components/ReabastecerStock";
import GestionUsuario from './components/GestionUsuario';
import PerfilUsuario from "./components/PerfilUsuario";
import ListaDeseos from "./components/ListaDeseos";
import CarritoCompras from "./components/CarritoCompras";
import HistorialCompras from "./components/HistorialCompras";
import HistorialVentas from "./components/HistorialVentas";
import GestionEventos from "./components/GestionEventos";

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
                <Route path="/admin-login" element={<LoginAdmin setEstaLogueado={setEstaLogueado} />} /> {/* NUEVA RUTA */}
                <Route path="/usuarios" element={estaLogueado ? <Usuarios /> : <Navigate to="/no-autorizado" />} />
                <Route path="/no-autorizado" element={<NoAutorizado />} />
                <Route path="/usuarios/:id" element={<UsuarioDetalle />} />
                <Route path="/catalogo" element={<Catalogo />} />
                <Route path="/catalogo-admin" element={<GestionCatalogo />} />
                <Route path="/gestion-admin" element={estaLogueado ? <GestionAdmin /> : <Navigate to="/no-autorizado" />} />
                <Route path="/productos-admin" element={<ProductosAdmin />} />
                <Route path="/categorias" element={<Categorias />} />
                <Route path="/reabastecer-stock" element={<ReabastecerStock />} /> 
                <Route path="/gestion-usuario" element={<GestionUsuario />} />
                <Route path="/perfil" element={<PerfilUsuario />} />
                <Route path="/lista-deseos" element={<ListaDeseos />} />
                <Route path="/carrito-compras" element={<CarritoCompras />} />
                <Route path="/historial-compras" element={<HistorialCompras />} />
                <Route path="/historial-ventas" element={<HistorialVentas />} />
                <Route path="/eventos" element={<GestionEventos />} />


            </Routes>
        </Router>
    );
}

export default App;
