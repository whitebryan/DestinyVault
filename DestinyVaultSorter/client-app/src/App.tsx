
import React from "react";
import HomePage from './Pages/Home';
import VaultPage from "./Pages/VaultPage";
import {BrowserRouter as Router, Route, Routes} from "react-router-dom";

function App() {
  return (
      <Router>
        <Routes>
          <Route path="" element={<HomePage />} />
          <Route path="/vault" element={<VaultPage />} />
        </Routes>
      </Router>
  );
}

export default App;
