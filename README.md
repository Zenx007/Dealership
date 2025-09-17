# 🚗🏍️ Concessionária CMD

Projeto em **C# (.NET Console Application)** que simula o funcionamento de uma **loja de veículos (carros e motos)**.  

Este projeto foi desenvolvido como **trabalho de faculdade** e tem como objetivo praticar conceitos de **Programação Orientada a Objetos (POO)**, **manipulação de arquivos**, **listas genéricas (List<T>)** e **interação via console**.

---

## 📌 Funcionalidades

- Listagem de **Carros** e **Motos**.
- Adição de produtos ao **Carrinho de Compras** (com escolha da quantidade).
- Remoção de produtos do carrinho (parcial ou total, com devolução ao estoque).
- Controle de **Estoque**.
- Finalização de compra com cálculo do **valor total**.
- Registro automático em arquivos `.txt`:
  - `carrinho.txt`: itens que estão no carrinho.
  - `vendas.txt`: histórico de todas as vendas realizadas.
- Inclusão de novos produtos no estoque diretamente pelo sistema.
- Visualização de todas as vendas já feitas.

---

## 📂 Estrutura de Arquivos

- `Program.cs` → Arquivo principal com toda a lógica do sistema.
- `carrinho.txt` → Lista de itens adicionados ao carrinho.
- `vendas.txt` → Histórico das vendas realizadas.

---

## ▶️ Como Executar

1. **Clone ou baixe** este repositório.
2. Abra o projeto em um editor/IDE compatível com C# (Visual Studio, Rider ou VS Code).
3. Compile e execute o programa:
   ```bash
   dotnet run
