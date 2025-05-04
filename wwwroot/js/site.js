function renderAST(ast) {
    const container = document.getElementById('ast-container');
    if (!container) return;
    
    container.innerHTML = '';
    const tree = document.createElement('div');
    tree.className = 'ast-tree';
    
    const rootNode = createNodeElement(ast);
    tree.appendChild(rootNode);
    container.appendChild(tree);
}

function createNodeElement(node) {
    const nodeElement = document.createElement('div');
    nodeElement.className = `ast-node node-${node.type.toLowerCase()}`;
    
    const nodeType = document.createElement('div');
    nodeType.className = 'ast-node-type';
    nodeType.textContent = node.type;
    nodeElement.appendChild(nodeType);
    
    switch (node.type) {
        case 'Constant':
            addProperty(nodeElement, 'Type', node.nodeType);
            addProperty(nodeElement, 'Value', node.value);
            break;
            
        case 'Variable':
            addProperty(nodeElement, 'Name', node.name);
            break;
            
        case 'BinaryOp':
            addProperty(nodeElement, 'Operator', node.op);
            addChildNode(nodeElement, 'Left', node.left);
            addChildNode(nodeElement, 'Right', node.right);
            break;
            
        case 'Function':
            addProperty(nodeElement, 'Parameter', node.parameter);
            addChildNode(nodeElement, 'Body', node.body);
            break;
            
        case 'Application':
            addChildNode(nodeElement, 'Function', node.function);
            addChildNode(nodeElement, 'Argument', node.argument);
            break;
            
        case 'IfThenElse':
            addChildNode(nodeElement, 'Condition', node.condition);
            addChildNode(nodeElement, 'Then', node.thenBranch);
            addChildNode(nodeElement, 'Else', node.elseBranch);
            break;
            
        case 'LetBinding':
            addProperty(nodeElement, 'Variable', node.variable);
            addChildNode(nodeElement, 'Definition', node.definition);
            addChildNode(nodeElement, 'Body', node.body);
            break;
    }
    
    return nodeElement;
}

function addProperty(parentElement, name, value) {
    const prop = document.createElement('div');
    prop.className = 'node-property';
    prop.textContent = `${name}: ${value}`;
    parentElement.appendChild(prop);
}

function addChildNode(parentElement, name, childNode) {
    const container = document.createElement('div');
    container.className = 'node-children';
    
    const label = document.createElement('div');
    label.className = 'node-property';
    label.textContent = name + ':';
    container.appendChild(label);
    
    const childElement = createNodeElement(childNode);
    container.appendChild(childElement);
    
    parentElement.appendChild(container);
}
