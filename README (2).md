# Guia de Configuração: Projeto Cadastro de Alimentos (MAUI + API + Docker/WSL)

Este guia descreve os passos necessários para configurar e executar o projeto completo em uma nova máquina Windows, utilizando Docker rodando diretamente no WSL 2 (sem Docker Desktop).

## 1. Pré-requisitos (Instalações Necessárias)

Antes de começar, certifique-se de que os seguintes softwares estão instalados na máquina:

1.  **WSL 2 com Ubuntu:**
    * Abra o **PowerShell como Administrador** e rode:
        ```powershell
        wsl --install
        ```
    * Siga as instruções para criar um usuário e senha para o Ubuntu.
2.  **.NET 9 SDK (ou a versão exata utilizada no projeto):**
    * Baixe e instale o SDK completo (inclui Runtime, ASP.NET Core, MAUI) do site oficial: [https://dotnet.microsoft.com/download/dotnet/9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
    * Verifique a instalação abrindo um PowerShell normal e rodando `dotnet --version`.
3.  **Git:**
    * Instale o Git para Windows: [https://git-scm.com/download/win](https://git-scm.com/download/win)
4.  **Visual Studio Code (Recomendado):**
    * Instale o VS Code: [https://code.visualstudio.com/](https://code.visualstudio.com/)
    * Extensões úteis: C#, Docker.
5.  **(Opcional) SQL Server Management Studio (SSMS):**
    * Útil para visualizar o banco de dados diretamente: [Download SSMS](https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms)

---

## 2. Clonar o Repositório

1.  Abra um terminal (Git Bash, PowerShell ou CMD).
2.  Navegue até a pasta onde deseja guardar o projeto (Ex: `C:\Projetos`).
3.  Clone o repositório do Git:
    ```bash
    git clone <URL_DO_SEU_REPOSITORIO_GIT>
    cd <NOME_DA_PASTA_DO_PROJETO> # Ex: cd mauiAlimentosFinal
    ```

---

## 3. Configurar Docker e WSL (no Ubuntu)

Esta parte é feita **dentro do terminal do Ubuntu (WSL)**.

1.  **Instalar Docker Engine no Ubuntu:**
    * Siga **todos** os comandos do **Passo 1** do `README.md` original do projeto (ou [veja a documentação oficial do Docker](https://docs.docker.com/engine/install/ubuntu/)):
        * `sudo apt-get update && sudo apt-get upgrade -y`
        * Instalar pré-requisitos (`ca-certificates`, `curl`, `gnupg`).
        * Adicionar chave GPG do Docker.
        * Adicionar repositório do Docker.
        * `sudo apt-get update`
        * `sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin`
        * `sudo usermod -aG docker $USER`
    * **IMPORTANTE:** Feche o terminal do Ubuntu e abra-o novamente após `usermod`.
2.  **Configurar Memória do WSL:**
    * No Windows, navegue até `%USERPROFILE%` (Ex: `C:\Users\SeuUsuario`).
    * Crie um arquivo chamado `.wslconfig` (sem extensão).
    * Adicione o seguinte conteúdo e salve:
        ```ini
        [wsl2]
        memory=4GB
        ```
    * Abra o **PowerShell (Windows)** e rode `wsl --shutdown`.
3.  **Ativar Systemd:**
    * Abra o terminal **Ubuntu (WSL)**.
    * Edite o arquivo de configuração do WSL:
        ```bash
        sudo nano /etc/wsl.conf
        ```
    * Adicione o seguinte conteúdo e salve (`Ctrl+O`, `Enter`, `Ctrl+X`):
        ```ini
        [boot]
        systemd=true
        ```
    * Abra o **PowerShell (Windows)** e rode `wsl --shutdown`.
    * Reabra o terminal Ubuntu. O Docker deve iniciar automaticamente. Verifique com `sudo systemctl status docker`. Se não estiver ativo, use `sudo systemctl start docker`.

---

## 4. Configurar Segredos e IP (`.env`)

Esta é a principal configuração manual.

1.  Na pasta **raiz** do projeto (Ex: `mauiAlimentosFinal`), localize o arquivo `.env.example`.
2.  **Copie** este arquivo e renomeie a cópia para `.env`.
3.  Abra o terminal **Ubuntu (WSL)** e descubra o IP atual:
    ```bash
    ip addr show eth0
    ```
4.  Anote o endereço IP que aparece na linha `inet` (Ex: `172.X.X.X`).
5.  **Edite o arquivo `.env`** no VS Code ou Bloco de Notas:
    * Defina uma senha forte para `SA_PASSWORD=sua_senha_forte_aqui`.
    * Cole o IP do WSL que você anotou em `WSL_IP=seu_ip_aqui`.
    * Mantenha `DB_DATABASE=CadastroAlimentosDB`.
6.  Salve o arquivo `.env`.

---

## 5. Iniciar Backend (API e Banco)

1.  Abra o terminal **Ubuntu (WSL)**.
2.  Navegue até a pasta **raiz** do projeto (onde está o `docker-compose.yml`).
    ```bash
    # Exemplo (ajuste o caminho):
    cd /mnt/c/Users/SeuUsuario/Caminho/Para/mauiAlimentosFinal
    ```
3.  Execute o Docker Compose:
    ```bash
    docker compose up --build -d
    ```
    * `--build`: Garante que a imagem da API seja (re)construída se o código mudou.
    * `-d`: Roda em segundo plano (detached).
4.  Aguarde cerca de 1-2 minutos para os serviços iniciarem completamente. Verifique o status com:
    ```bash
    docker ps
    ```
    * Devem aparecer `mssql-dev` e `api-dev` com status "Up". O `mssql-dev` pode levar mais tempo para mostrar "(healthy)".

---

## 6. Criar o Banco de Dados (Migrations)

1.  Abra um terminal **PowerShell** (no Windows).
2.  Navegue até a pasta do projeto da **API**:
    ```powershell
    # Exemplo:
    cd C:\Users\SeuUsuario\Caminho\Para\mauiAlimentosFinal\CadastroAlimentos.Api
    ```
3.  Instale o pacote DotNetEnv (necessário para ler o `.env`):
    ```powershell
    dotnet add package DotNetEnv
    ```
4.  Execute o comando para aplicar as migrations (ele lerá o `.env` para conectar):
    ```powershell
    dotnet ef database update
    ```
    * Isso deve criar o banco `CadastroAlimentosDB` e a tabela `Alimentos` no contêiner `mssql-dev`.

---

## 7. (Opcional) Testar a API

1.  Abra um navegador no Windows e acesse `http://localhost:8080/swagger` ou `http://localhost:8080/api/alimentos`.
2.  Você deve ver a página do Swagger ou uma lista vazia `[]`. Se der erro, verifique se os contêineres estão rodando (`docker ps`) e se o IP no `.env` está correto.

---

## 8. Rodar o Aplicativo MAUI

1.  Abra um terminal **PowerShell** (no Windows).
2.  Navegue até a pasta do projeto **MAUI**:
    ```powershell
    # Exemplo:
    cd C:\Users\SeuUsuario\Caminho\Para\mauiAlimentosFinal\CadastroAlimentos9
    ```
3.  Instale o pacote DotNetEnv (necessário para ler o `.env`):
    ```powershell
    dotnet add package DotNetEnv
    ```
4.  Execute o comando para rodar o app no Windows (ele lerá o `.env` para conectar):
    ```powershell
    dotnet build -t:Run -f net9.0-windows10.0.19041.0
    ```
5.  O aplicativo deve abrir.

---

## 9. Permissão de Rede do MAUI (Se Crashar ao Salvar)

Se o aplicativo MAUI abrir, mas **fechar sozinho** (crashar) **exatamente** quando você clicar em "Salvar Alimento", o Windows está bloqueando a conexão de rede local. Siga estes passos **uma única vez** na máquina:

1.  Compile o MAUI (sem rodar):
    ```powershell
    # Na pasta CadastroAlimentos9
    dotnet build -f net9.0-windows10.0.19041.0
    ```
2.  Rode o `.exe` manualmente: Navegue até `bin\Debug\net9.0-windows10.0.19041.0\win10-x64\` e clique duas vezes em `CadastroAlimentos9.exe`. Mantenha o app aberto (mesmo que pareça travado).
3.  Abra um **novo PowerShell (como Administrador)**.
4.  Liste os SIDs dos apps rodando:
    ```powershell
    CheckNetIsolation LoopbackExempt -s
    ```
5.  Encontre o nome parecido com `cadastroalimentos9...` na lista e copie o `SID do Contêiner de Aplicativo` correspondente (`S-1-15-2...`).
6.  Adicione a exceção de rede (substitua `SEU_SID_AQUI`):
    ```powershell
    CheckNetIsolation LoopbackExempt -a -p="SEU_SID_AQUI"
    ```
    * Deve retornar "OK."
7.  Feche o app MAUI e tente rodá-lo novamente com `dotnet build -t:Run...`. O crash ao salvar deve parar.

---

Seguindo estes passos, outra pessoa deve conseguir configurar todo o ambiente e rodar seu projeto completo.
