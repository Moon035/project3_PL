// AST Visualization functions
function renderAST(ast) {
    const container = document.getElementById('ast-container');
    if (!container) {
        console.error('AST container not found');
        return;
    }
    
    // Clear previous content
    container.innerHTML = '';
    
    // Log the AST for debugging
    console.log('Rendering AST:', ast);
    
    // Create tree container
    const tree = document.createElement('div');
    tree.className = 'tree';
    
    try {
        const ul = document.createElement('ul');
        const li = document.createElement('li');
        
        // Create node element
        const nodeElement = createNodeElement(ast);
        li.appendChild(nodeElement);
        
        // Add children if they exist
        const childrenUl = createChildrenElement(ast);
        if (childrenUl) {
            li.appendChild(childrenUl);
        }
        
        ul.appendChild(li);
        tree.appendChild(ul);
        container.appendChild(tree);
    } catch (error) {
        console.error('Error creating AST visualization:', error);
        container.innerHTML = '<div class="error-panel">Error rendering AST: ' + error.message + '</div>';
    }
}

function createNodeElement(node) {
    if (!node) {
        console.error('Node is null or undefined');
        return document.createElement('div');
    }

    const nodeDiv = document.createElement('div');
    nodeDiv.className = `node node-${(node.type || '').toLowerCase()}`;
    
    // Create the node type header
    const nodeType = document.createElement('div');
    nodeType.className = 'node-type';
    nodeType.textContent = node.type;
    nodeDiv.appendChild(nodeType);
    
    // Add node properties based on the node type
    switch (node.type) {
        case 'Constant':
            addProperty(nodeDiv, `Type: ${node.nodeType}`);
            addProperty(nodeDiv, `Value: ${node.value}`);
            break;
            
        case 'Variable':
            addProperty(nodeDiv, `Name: ${node.name}`);
            break;
            
        case 'BinaryOp':
            addProperty(nodeDiv, `Operator: ${node.op}`);
            break;
            
        case 'Function':
            addProperty(nodeDiv, `Parameter: ${node.parameter}`);
            break;
            
        case 'LetBinding':
            addProperty(nodeDiv, `Variable: ${node.variable}`);
            break;
            
        case 'Application':
            // Application nodes typically don't need properties
            break;
            
        case 'IfThenElse':
            // If-then-else nodes typically don't need properties
            break;
    }
    
    return nodeDiv;
}

function createChildrenElement(node) {
    const children = getChildNodes(node);
    
    if (children.length === 0) {
        return null;
    }
    
    const ul = document.createElement('ul');
    
    children.forEach(child => {
        const li = document.createElement('li');
        
        // Create child node
        const childNode = createNodeElement(child.node);
        li.appendChild(childNode);
        
        // Recursively add children
        const childrenUl = createChildrenElement(child.node);
        if (childrenUl) {
            li.appendChild(childrenUl);
        }
        
        ul.appendChild(li);
    });
    
    return ul;
}

function getChildNodes(node) {
    const children = [];
    
    console.log('Getting children for node:', node.type);
    
    switch (node.type) {
        case 'BinaryOp':
            if (node.left) children.push({ label: 'Left', node: node.left });
            if (node.right) children.push({ label: 'Right', node: node.right });
            break;
            
        case 'Function':
            if (node.body) children.push({ label: 'Body', node: node.body });
            break;
            
        case 'Application':
            if (node.function) children.push({ label: 'Function', node: node.function });
            if (node.argument) children.push({ label: 'Argument', node: node.argument });
            break;
            
        case 'IfThenElse':
            if (node.condition) children.push({ label: 'Condition', node: node.condition });
            if (node.thenBranch) children.push({ label: 'Then', node: node.thenBranch });
            if (node.elseBranch) children.push({ label: 'Else', node: node.elseBranch });
            break;
            
        case 'LetBinding':
            if (node.definition) children.push({ label: 'Definition', node: node.definition });
            if (node.body) children.push({ label: 'Body', node: node.body });
            break;
    }
    
    console.log('Children for', node.type, ':', children);
    
    return children;
}

function addProperty(parentElement, text) {
    const prop = document.createElement('div');
    prop.className = 'node-property';
    prop.textContent = text;
    parentElement.appendChild(prop);
}

// Document ready handler
document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM loaded');
    
    // Set up example buttons
    const exampleButtons = document.querySelectorAll('.btn-example');
    exampleButtons.forEach(button => {
        button.addEventListener('click', function() {
            const code = this.getAttribute('data-example');
            const codeInput = document.getElementById('codeInput');
            if (codeInput) {
                codeInput.value = code;
            }
        });
    });
});