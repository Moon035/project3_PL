// AST Visualization functions
function renderAST(ast) {
    const container = document.getElementById('ast-container');
    if (!container) {
        console.error('AST container not found');
        return;
    }
    
    container.innerHTML = '';
    const tree = document.createElement('div');
    tree.className = 'ast-tree';
    
    try {
        const rootNode = createNodeElement(ast);
        tree.appendChild(rootNode);
        container.appendChild(tree);
    } catch (error) {
        console.error('Error creating AST visualization:', error);
        container.innerHTML = '<div class="error-panel">Error rendering AST</div>';
    }
}

function createNodeElement(node) {
    if (!node) {
        console.error('Node is null or undefined');
        return document.createElement('div');
    }

    const nodeElement = document.createElement('div');
    nodeElement.className = `ast-node node-${(node.type || '').toLowerCase()}`;
    
    // Create the node type header
    const nodeType = document.createElement('div');
    nodeType.className = 'ast-node-type';
    nodeType.textContent = node.type;
    nodeElement.appendChild(nodeType);
    
    // Add node properties based on the node type
    switch (node.type) {
        case 'Constant':
            addProperty(nodeElement, `Type: ${node.nodeType}`);
            addProperty(nodeElement, `Value: ${node.value}`);
            break;
            
        case 'Variable':
            addProperty(nodeElement, `Name: ${node.name}`);
            break;
            
        case 'BinaryOp':
            addProperty(nodeElement, `Operator: ${node.op}`);
            // Create children container
            const binaryChildren = document.createElement('div');
            binaryChildren.className = 'node-children';
            if (node.left) binaryChildren.appendChild(createNodeElement(node.left));
            if (node.right) binaryChildren.appendChild(createNodeElement(node.right));
            nodeElement.appendChild(binaryChildren);
            break;
            
        case 'Function':
            addProperty(nodeElement, `Parameter: ${node.parameter}`);
            const functionChildren = document.createElement('div');
            functionChildren.className = 'node-children';
            if (node.body) functionChildren.appendChild(createNodeElement(node.body));
            nodeElement.appendChild(functionChildren);
            break;
            
        case 'Application':
            const appChildren = document.createElement('div');
            appChildren.className = 'node-children';
            if (node.function) appChildren.appendChild(createNodeElement(node.function));
            if (node.argument) appChildren.appendChild(createNodeElement(node.argument));
            nodeElement.appendChild(appChildren);
            break;
            
        case 'IfThenElse':
            const ifChildren = document.createElement('div');
            ifChildren.className = 'node-children';
            
            // Create labeled children for if-then-else
            if (node.condition) {
                const conditionWrapper = createLabeledChild('condition', node.condition);
                ifChildren.appendChild(conditionWrapper);
            }
            if (node.thenBranch) {
                const thenWrapper = createLabeledChild('then', node.thenBranch);
                ifChildren.appendChild(thenWrapper);
            }
            if (node.elseBranch) {
                const elseWrapper = createLabeledChild('else', node.elseBranch);
                ifChildren.appendChild(elseWrapper);
            }
            
            nodeElement.appendChild(ifChildren);
            break;
            
        case 'LetBinding':
            addProperty(nodeElement, `Variable: ${node.variable}`);
            const letChildren = document.createElement('div');
            letChildren.className = 'node-children';
            if (node.definition) letChildren.appendChild(createNodeElement(node.definition));
            if (node.body) letChildren.appendChild(createNodeElement(node.body));
            nodeElement.appendChild(letChildren);
            break;
    }
    
    return nodeElement;
}

function createLabeledChild(label, childNode) {
    const wrapper = document.createElement('div');
    wrapper.style.textAlign = 'center';
    
    const labelElement = document.createElement('div');
    labelElement.style.fontSize = '12px';
    labelElement.style.color = '#666';
    labelElement.style.marginBottom = '5px';
    labelElement.textContent = label;
    
    wrapper.appendChild(labelElement);
    if (childNode) {
        wrapper.appendChild(createNodeElement(childNode));
    }
    
    return wrapper;
}

function addProperty(parentElement, text) {
    const prop = document.createElement('div');
    prop.className = 'node-property';
    prop.textContent = text;
    parentElement.appendChild(prop);
}