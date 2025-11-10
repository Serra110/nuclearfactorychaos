# 🔧 Configuração do Sistema de Pickup

## ✅ Checklist - O que precisa configurar no Unity:

### 1. **Script no Player**
- ✅ O script `PlayerPickupSimple` deve estar anexado ao GameObject do Player
- ✅ Verifica no Inspector se o script está lá

### 2. **HoldPoint (OBRIGATÓRIO)**
- ✅ Cria um GameObject vazio (GameObject > Create Empty)
- ✅ Nomeia-o como "HoldPoint"
- ✅ Posiciona-o à frente da câmera (ex: posição Z = 2, Y = 0, X = 0)
- ✅ **IMPORTANTE**: Arrasta este GameObject para o campo "Hold Point" no Inspector do script PlayerPickupSimple
- ✅ Ou faz o HoldPoint ser filho da câmera para seguir o movimento

### 3. **Câmera Principal**
- ✅ Certifica-te de que há uma câmera na cena
- ✅ A câmera DEVE ter a tag "MainCamera" (não "Main Camera" com espaço!)
- ✅ Verifica: Seleciona a câmera > Inspector > Tag dropdown > deve estar "MainCamera"

### 4. **Objetos que Queres Pegar**
Cada objeto que queres pegar precisa de:
- ✅ **Tag "Pickup"**: 
  - Seleciona o objeto
  - Inspector > Tag dropdown > "Pickup" (se não existir, cria uma nova tag)
- ✅ **Rigidbody**:
  - Add Component > Rigidbody
  - Podes deixar as configurações padrão
- ✅ **Collider**:
  - O objeto precisa de um Collider (Box Collider, Sphere Collider, etc.)
  - O Collider deve estar ativado (checkbox marcado)

### 5. **Testar**
1. Pressiona Play
2. Aproxima-te de um objeto com tag "Pickup"
3. Olha para o objeto (centro do ecrã)
4. Pressiona **E**
5. Verifica o Console para ver as mensagens de debug

## 🐛 Troubleshooting

Se não funcionar, verifica no Console:
- ❌ "Camera.main não encontrada" → Verifica a tag da câmera
- ❌ "holdPoint não está atribuído" → Arrasta o HoldPoint para o campo no Inspector
- ❌ "Raycast não acertou nada" → Estás muito longe ou não estás a olhar para o objeto
- ❌ "não tem a tag 'Pickup'" → Adiciona a tag "Pickup" ao objeto
- ❌ "não tem Rigidbody" → Adiciona um Rigidbody ao objeto

