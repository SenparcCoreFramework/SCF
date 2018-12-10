import React, { Component } from 'react';
//import { Link } from 'react-router-dom';
//import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import { Layout, Menu, Icon } from 'antd';
import './NavMenu.css';
const {
    Sider
} = Layout;

export class NavMenu extends Component {
    displayName = NavMenu.name
    render() {
        return (
            <Sider
                style={{
                    overflow: 'auto', height: '100vh', position: 'fixed', left: 0,
                }}>
                    <div className="logo" />
                    <Menu theme="dark" mode="inline" defaultSelectedKeys={['4']}>
                        <Menu.Item key="1">
                            <LinkContainer to={'/'} exact>
                                <a>
                                    <Icon type="user" />
                                    <span className="nav-text">首页</span>
                                </a>
                            </LinkContainer>
                        </Menu.Item>
                        <Menu.Item key="2">
                            <LinkContainer to={'/counter'}>
                                <a>
                                    <Icon type="video-camera" />
                                    <span className="nav-text">管理员管理</span>
                                </a>
                            </LinkContainer>
                        </Menu.Item>
                        <Menu.Item key="3">
                            <LinkContainer to={'/fetchdata'}>
                                <a>
                                    <Icon type="upload" />
                                    <span className="nav-text">日志</span>
                                </a>
                            </LinkContainer>
                        </Menu.Item>
                    </Menu>
            </ Sider>
                );
            }
        }
