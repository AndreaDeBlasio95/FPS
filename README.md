## ReadMe Documentation for Unity FPS Task

## Requirement: Unity 2022.3.19f1 <Metal>

### Scene Setup
- **Initial Composition**: The scene is pre-populated with all levels and the player character, ready for gameplay.

### Hierarchy and Core Components

#### Player Management
- **PlayerInputHandler**
  - **Location**: `Assets -> InputSystemControls -> PlayerControls`
  - **Function**: Manages player input using Unity's new Input System, ensuring responsive controls.

- **FPSController**
  - **Components**: Includes the HealthPlayer script, which inherits from CharacterHealth. This component utilizes a Scriptable Object for defining player and enemy data.
  - **Description**: Handles player movement. Attached to this GameObject is the Main Camera which uses CameraAnimatorManager to manage motion and aiming animations.

#### Camera and Animations
- **Main Camera**
  - **WeaponAnimationController**: Manages the animations for weapon recoil.
    - **WeaponAnimationContainer**
      - **WeaponContainer**
      - **SpawnPositionBullet**
        - **FX_Shoot_01_MuzzleFlash**: Responsible for muzzle flash effects when firing.

#### Level Management
- **LevelManager**
  - **Location**: `Assets -> Scripts -> LevelManager`
  - **Function**: Manages references to individual levels and checks victory conditions based on the count of enemies left. It also updates the UI with information about the current level and the enemies remaining.

- **Levels**
  - **WaitingRoom**: Initial area for player entry.
  - **Level-1** and **Level-2**
    - **Script**: `Level`
    - **Function**: Each level's script is referenced by all enemies within that scene. These enemies invoke the `EnemyKilled` method to update the LevelManager about changes in enemy counts.

#### UI Management
- **UIController**
  - **Location**: `Assets -> Scripts -> UIController`
  - **Function**: Manages all UI elements related to player stats and weapon data.

#### Weapon Management
- **WeaponManager**
  - **Location**: `Assets -> Scripts -> WeaponManager`
  - **Function**: Oversees the creation and update of weapon logic, including shooting and aiming functionalities.

#### Object Pooling
- **BulletPool**
  - **Location**: `Assets -> Scripts -> BulletPool`
  - **Function**: Implements the Object Pool Pattern to optimize performance by managing ammunition through a stack, reducing the need for runtime instantiation.

#### Enemies and AI
- **Enemy Instances**
  - **Components**: Include EnemyHealth, which inherits from CharacterHealth, and EnemyAIMovement utilizing NavMesh for navigation.
  - **Sub-Components**: **EnemyColliderDetector**
    - **Function**: Detects when the player is within actionable range to trigger enemy responses.

#### Power Ups
- **Types**: Shield and Health
  - **Description**: Strategically placed within the scene to provide timely boosts to the player, enhancing survival and gameplay dynamics.

#### Scriptable Objects
- **Location**: `Assets -> Scriptable Objects`
- **Description**: This directory contains all Scriptable Object files which are used for managing reusable data like player health, enemy configurations, and other adjustable game parameters.

### Scalability Note
- While the current setup includes all levels in a single scene for simplicity, the architecture supports scalable implementations. Levels can be instantiated in an infinite loop or expanded in any direction, leveraging a list within LevelManager for managing spawn positions or iterating through similarly sized rooms using Vector3 multiplication by index.
